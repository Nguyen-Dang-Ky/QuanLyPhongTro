using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuanLyPhongTro.dtos.BillDTOs;
using QuanLyPhongTro.dtos;
using QuanLyPhongTro.services.Interfaces;
using QuanLyPhongTro.data;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using QuanLyPhongTro.model;
using Microsoft.EntityFrameworkCore;
using QuanLyPhongTro.dtos.CustomerDTOs;
public class PaymentService : IPaymentService
{
    private readonly ManagementDbContext _context;

    public PaymentService(ManagementDbContext context)
    {
        _context = context;
    }

    // ================= CREATE =================
    public async Task<PaymentResponseDto> CreateAsync(PaymentCreateDto dto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var bill = await _context.Bills
                .Include(b => b.Payments)
                .FirstOrDefaultAsync(b => b.BillID == dto.BillID);

            if (bill == null)
                throw new Exception("Bill not found");

            if (dto.AmountPaid <= 0)
                throw new Exception("Amount must be greater than 0");

            decimal totalPaid = bill.Payments.Sum(p => p.AmountPaid);
            decimal remaining = bill.TotalAmount - totalPaid;

            if (dto.AmountPaid > remaining)
                throw new Exception("Payment exceeds remaining amount");

            var payment = new Payments
            {
                BillID = dto.BillID,
                AmountPaid = dto.AmountPaid,
                PaymentMethod = dto.PaymentMethod,
                PaymentDate = dto.PaymentDate,
                TransactionCode = dto.TransactionCode
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            await UpdateBillStatusAsync(dto.BillID);

            await transaction.CommitAsync();

            return MapToResponse(payment);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    // ================= UPDATE =================
    public async Task<PaymentResponseDto?> UpdateAsync(int id, PaymentUpdateDto dto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null) return null;

            var bill = await _context.Bills
                .Include(b => b.Payments)
                .FirstOrDefaultAsync(b => b.BillID == payment.BillID);

            if (bill == null)
                throw new Exception("Bill not found");

            if (dto.AmountPaid <= 0)
                throw new Exception("Amount must be greater than 0");

            // tính lại totalPaid (trừ payment cũ ra)
            decimal totalPaid = bill.Payments
                .Where(p => p.PaymentID != id)
                .Sum(p => p.AmountPaid);

            decimal remaining = bill.TotalAmount - totalPaid;

            if (dto.AmountPaid > remaining)
                throw new Exception("Payment exceeds remaining amount");

            payment.AmountPaid = dto.AmountPaid;
            payment.PaymentMethod = dto.PaymentMethod;
            payment.PaymentDate = dto.PaymentDate;
            payment.TransactionCode = dto.TransactionCode;

            await _context.SaveChangesAsync();

            await UpdateBillStatusAsync(payment.BillID);

            await transaction.CommitAsync();

            return MapToResponse(payment);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    // ================= DELETE =================
    public async Task<bool> DeleteAsync(int id)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null) return false;

            int billId = payment.BillID;

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();

            await UpdateBillStatusAsync(billId);

            await transaction.CommitAsync();

            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    // ================= GET =================
    public async Task<PaymentResponseDto?> GetByIdAsync(int id)
    {
        var payment = await _context.Payments.FindAsync(id);
        return payment == null ? null : MapToResponse(payment);
    }

    public async Task<IEnumerable<PaymentResponseDto>> GetByBillIdAsync(int billId)
    {
        var list = await _context.Payments
            .Where(p => p.BillID == billId)
            .ToListAsync();

        return list.Select(MapToResponse);
    }

    public async Task<IEnumerable<PaymentResponseDto>> GetAllAsync()
    {
        var list = await _context.Payments.ToListAsync();
        return list.Select(MapToResponse);
    }

    // ================= PRIVATE =================

    private async Task UpdateBillStatusAsync(int billId)
    {
        var bill = await _context.Bills
            .Include(b => b.Payments)
            .FirstOrDefaultAsync(b => b.BillID == billId);

        if (bill == null) return;

        decimal totalPaid = bill.Payments.Sum(p => p.AmountPaid);

        if (totalPaid == 0)
        {
            bill.Status = BillStatus.Unpaid;
        }
        else if (totalPaid < bill.TotalAmount)
        {
            bill.Status = BillStatus.Unpaid; // hoặc Partial nếu bạn có enum
        }
        else
        {
            bill.Status = BillStatus.Paid;
        }

        await _context.SaveChangesAsync();
    }

    private static PaymentResponseDto MapToResponse(Payments p)
    {
        return new PaymentResponseDto
        {
            PaymentID = p.PaymentID,
            BillID = p.BillID,
            AmountPaid = p.AmountPaid,
            PaymentMethod = p.PaymentMethod,
            PaymentDate = p.PaymentDate,
            TransactionCode = p.TransactionCode
        };
    }
}