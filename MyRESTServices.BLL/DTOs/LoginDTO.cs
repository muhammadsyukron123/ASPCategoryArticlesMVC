using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyRESTServices.BLL.DTOs
{
	public class LoginDTO
	{
		public string Username { get; set; }

		public string Password { get; set; }
	}
}
