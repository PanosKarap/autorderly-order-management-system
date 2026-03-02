using FirstProjectAPI.Data;
using FirstProjectAPI.DTOs.Supplier;
using FirstProjectAPI.Interfaces;
using FirstProjectAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstProjectAPI.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly AppDbContext _context;

        public SupplierService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsAsync(string userId, string name, int? excludeId = null)
        {
            // Εδώ ψάχνουμε μόνο στους μη διαγραμμένους
            return await _context.Suppliers
                .AnyAsync(s => s.UserId == userId &&
                               s.Name.ToLower() == name.ToLower() &&
                               (!excludeId.HasValue || s.Id != excludeId.Value));
        }

        public async Task<IEnumerable<SupplierResponseDto>> GetAllAsync(string userId)
        {
            // Το φίλτρο !IsDeleted μπαίνει αυτόματα από το DbContext
            return await _context.Suppliers
                .Where(s => s.UserId == userId)
                .Select(s => new SupplierResponseDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Address = s.Address,
                    Email = s.Email,
                    PhoneNumber = s.PhoneNumber
                })
                .ToListAsync();
        }

        public async Task<SupplierResponseDto> CreateAsync(SupplierRequestDto dto, string userId)
        {
            if (await ExistsAsync(userId, dto.Name))
                throw new InvalidOperationException("Υπάρχει ήδη προμηθευτής με αυτό το όνομα.");

            var supplier = new Supplier
            {
                Name = dto.Name,
                Address = dto.Address,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                UserId = userId
            };

            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();

            return new SupplierResponseDto { Id = supplier.Id, Name = supplier.Name };
        }

        public async Task<SupplierResponseDto> UpdateAsync(int id, SupplierRequestDto dto, string userId)
        {
            var supplier = await _context.Suppliers
                .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);

            if (supplier == null) throw new KeyNotFoundException("Ο προμηθευτής δεν βρέθηκε.");

            if (await ExistsAsync(userId, dto.Name, id))
                throw new InvalidOperationException("Το όνομα χρησιμοποιείται ήδη.");

            supplier.Name = dto.Name;
            supplier.Address = dto.Address;
            supplier.Email = dto.Email;
            supplier.PhoneNumber = dto.PhoneNumber;

            await _context.SaveChangesAsync();
            return new SupplierResponseDto { Id = supplier.Id, Name = supplier.Name };
        }

        public async Task DeleteAsync(int id, string userId)
        {
            var supplier = await _context.Suppliers
                .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);

            if (supplier == null) throw new KeyNotFoundException("Ο προμηθευτής δεν βρέθηκε.");

            supplier.IsDeleted = true;

            await _context.SaveChangesAsync();
        }
    }
}