
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由T4模板自动生成
//     生成时间 2018-01-25 17:40:21
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;
namespace FPJ.Model.Default
{   

		 /// <summary>
		///实体-User 
		/// <summary>
		public partial class User
		{
		
			/// <summary>
			///  
			/// </summary>
			[Key]
			public string  UserId {get;set;}
			
			/// <summary>
			///  
			/// </summary>
			
			public string  UserName {get;set;}
			
			/// <summary>
			///  
			/// </summary>
			
			public string  Password {get;set;}
			
			/// <summary>
			///  
			/// </summary>
			
			public string  ConnectionId {get;set;}
			
			/// <summary>
			///  
			/// </summary>
			
			public string  LastLoginTime {get;set;}
					
		}

}