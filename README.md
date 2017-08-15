# VinValidationAttribute
MVC Vehicle Identification Number Validator


```csharp
using System;
using shapemetrics.VinValidation;

namespace shapemetrics.models
{
	public class VehicleViewModel
	{
		[VinValidation]
		public string VIN{get;set;}

	}




}
```