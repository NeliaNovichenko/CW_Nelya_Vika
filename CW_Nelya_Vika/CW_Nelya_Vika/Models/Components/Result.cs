using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CW_Nelya_Vika.Models
{
    /// <summary>
    /// Клас, що місить підграфи - результат роботи алгоритму
    /// </summary>
    public class Result : List<Graph>
    {
        [Key]
        public int Id { get; set; }
    }
}