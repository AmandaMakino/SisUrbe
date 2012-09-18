//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

//Generated using version %PRODUCTVERSION% of the NHibernate DSL Tool

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace SysCEF.Model
{
	[Serializable]
	[DataContract]
	// ReSharper disable PartialTypeWithSinglePart
	public partial class Usuario
	// ReSharper restore PartialTypeWithSinglePart
	{
		#region Constructors

		partial void BeforeInitialized();
		partial void AfterInitialized();

		public Usuario()
		{
			BeforeInitialized();
			
			Nome = @"";
			Email = @"";
			Senha = @"";
			
			AfterInitialized();
		}

		#endregion Constructors

		#region Private Fields

		#pragma warning disable 0169

		[DataMember]
		// ReSharper disable InconsistentNaming
		///<summary>Field to facilitate optimistic locking</summary>
		private int __Version;
		// ReSharper restore InconsistentNaming

		#pragma warning restore 0169

		#endregion Private Fields

		#region Properties

		[DataMember]
		public virtual int UsuarioId { get; set; }

		[DataMember]
		public virtual string Nome { get; set; }

		[DataMember]
		public virtual int Perfil { get; set; }

		[DataMember]
		public virtual string Email { get; set; }

		[DataMember]
		public virtual string Senha { get; set; }

		[DataMember]
		public virtual bool DeveDefinirNovaSenha { get; set; }

		[DataMember]
		public virtual string CREA { get; set; }

		[DataMember]
		public virtual string CPF { get; set; }

		#endregion Properties

		#region Object Overrides

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			if (ReferenceEquals(this, obj))
				return true;

			if (GetType() != obj.GetType())
				return false;

			return UsuarioId <= 0 ? false : UsuarioId == ((Usuario) obj).UsuarioId;
		}

		public override int GetHashCode()
		{
			if (UsuarioId <= 0)
				return base.GetHashCode();

			//Use 37 as a multiplier as it is a relatively large prime number
			//which helps to avoid collisions in a hashed data structure
			return 37 * UsuarioId;
		}

		#endregion Object Overrides

	}
}
