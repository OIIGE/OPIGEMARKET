﻿namespace EliteMart.Helpers
{
    public class ProductQueryObject
    {
       
        public string? ProductName { get; set; } = null;
       
        public string?  Category {  get; set; } = null;
            
        public string? SortBy { get; set; } = null;

        public bool IsDecending { get; set; } = false;

      
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = int.MaxValue;

    }
}
