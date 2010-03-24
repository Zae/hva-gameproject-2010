/*
	FluorineFx open source library 
	Copyright (C) 2007 Zoltan Csibi, zoltan@TheSilentGroup.com, FluorineFx.com 
	
	This library is free software; you can redistribute it and/or
	modify it under the terms of the GNU Lesser General Public
	License as published by the Free Software Foundation; either
	version 2.1 of the License, or (at your option) any later version.
	
	This library is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
	Lesser General Public License for more details.
	
	You should have received a copy of the GNU Lesser General Public
	License along with this library; if not, write to the Free Software
	Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
*/
using System;
using System.Collections;
using System.IO;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Text;
using System.Xml;
using Microsoft.CSharp;

using log4net;

using FluorineFx.AMF3;
using FluorineFx.Exceptions;
using FluorineFx.Configuration;
using FluorineFx.Util;
using FluorineFx.IO.Readers;
namespace FluorineFx.IO.Bytecode.CodeDom
{
	/// <summary>
	/// This type supports the Fluorine infrastructure and is not intended to be used directly from your code.
	/// </summary>
	class BytecodeProvider : IBytecodeProvider
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(BytecodeProvider));

		public BytecodeProvider()
		{
		}

		#region IBytecodeProvider Members


        public IReflectionOptimizer GetReflectionOptimizer(Type type, ClassDefinition classDefinition, AMFReader reader, object instance)
        {
            if( classDefinition == null )
                return new AMF0ReflectionOptimizer(type, reader).Generate(instance);
            else
                return new AMF3ReflectionOptimizer(type, classDefinition, reader).Generate(instance);
        }

		#endregion

	}
}
