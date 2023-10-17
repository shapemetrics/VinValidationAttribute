# VinValidationAttribute

Vehicle Identification Number Validator for .net core models as an attribute. It is a simple algorithm designed to validate the check digit within a VIN. This only validates the format and the check digit, it does not check the World Manufacturer Identifier (WMI), plant, Vehicle Description Section, year or any other portion of the VIN. T


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
## Validate the postback
within your controller method for postback, check to see if the model is valid.

```csharp
if(!ModelState.IsValid){
	return View(model);
}


```

To include unobtrustive client side validation methods.

```html
<script src="/js/VinValidation.js"></script>

```
