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
	public partial class InfraEstruturaUrbana
	// ReSharper restore PartialTypeWithSinglePart
	{
		#region Constructors

		partial void BeforeInitialized();
		partial void AfterInitialized();

		public InfraEstruturaUrbana()
		{
			BeforeInitialized();
			
			Descricao = @"";
			
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
		public virtual int InfraEstruturaUrbanaID { get; set; }

		[DataMember]
		public virtual int TipoInfraEstruturaUrbana { get; set; }

		[DataMember]
		public virtual string Descricao { get; set; }

		[DataMember]
		public virtual Laudo Laudo { get; set; }

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

			return InfraEstruturaUrbanaID <= 0 ? false : InfraEstruturaUrbanaID == ((InfraEstruturaUrbana) obj).InfraEstruturaUrbanaID;
		}

		public override int GetHashCode()
		{
			if (InfraEstruturaUrbanaID <= 0)
				return base.GetHashCode();

			//Use 37 as a multiplier as it is a relatively large prime number
			//which helps to avoid collisions in a hashed data structure
			return 37 * InfraEstruturaUrbanaID;
		}

		#endregion Object Overrides

	}
}
