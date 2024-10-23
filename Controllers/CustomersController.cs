﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EliteMart.Data;
using EliteMart.DTOS.Customer;
using EliteMart.Interfaces;
using EliteMart.Mappers;
using EliteMart.Model;
using System;

namespace EliteMart.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ICustomerRepository _customerRepo;

        public CustomersController(AppDbContext context, ICustomerRepository customerRepo)
        {
            _customerRepo = customerRepo;
            _context = context;
        }

        //to return all the list of record in the database
        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            var customers = await _customerRepo.GetAllAsync();

            var CustomerDto = customers.Select(s => s.ToCustomerDto());
            return Ok(customers);
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customerModel = await _customerRepo.GetByIdAsync(id); ;

            if (customerModel == null)
            {
                return NotFound();
            }

            return Ok(customerModel.ToCustomerDto());
        }

        // POST: api/Customers CREATE
        [HttpPost]
        public async Task <IActionResult> Create([FromBody] CreateCustomerDto customerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerModel = customerDto.ToCustomerFromCustomerDto(); 
           await _customerRepo.CreateAsync(customerModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCustomer), new { id = customerModel.Id }, customerModel.ToCustomerDto());
        }


        // PUT: api/Customers/5 EDIT
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCustomerDto updatedDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerModel = await _customerRepo.UpdateAsync(id, updatedDto);
           
            if (customerModel == null)
            {
                return NotFound();
            }
                       
            return Ok(customerModel.ToCustomerDto());
        }


        // DELETE: api/Customers/5   DELETE
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customerModel = await _customerRepo.DeleteAsync(id);
            if (customerModel == null)
            {
                return NotFound();
            }

            

            return NoContent();
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}