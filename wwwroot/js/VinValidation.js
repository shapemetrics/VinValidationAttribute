'use strict';

var weights = [8, 7, 6, 5, 4, 3, 2, 10, 0, 9, 8, 7, 6, 5, 4, 3, 2];

var replaceValues = { 'A': 1, 'B': 2, 'C': 3, 'D': 4, 'E': 5, 'F': 6, 'G': 7, 'H': 8, 'J': 1, 'K': 2, 'L': 3, 'M': 4, 'N': 5, 'P': 7, 'R': 9, 'S': 2, 'T': 3, 'U': 4, 'V': 5, 'W': 6, 'X': 7, 'Y': 8, 'Z': 9, '1': 1, '2': 2, '3': 3, '4': 4, '5': 5, '6': 6, '7': 7, '8': 8, '9': 9, '0': 0 };

var yearValues = { 'A': 1, 'B': 2, 'C': 3, 'D': 4, 'E': 5, 'F': 6, 'G': 7, 'H': 8, 'J': 1, 'K': 2, 'L': 3, 'M': 4, 'N': 5, 'P': 7, 'R': 9, 'S': 2, 'T': 3, 'V': 5, 'W': 6, 'X': 7, 'Y': 8, '1': 1, '2': 2, '3': 3, '4': 4, '5': 5, '6': 6, '7': 7, '8': 8, '9': 9 };


function validateLength(vin) {
    return vin !== null && vin !== undefined && vin.length == 17;
}

function validateCheckDigit(vin) {

    if (!validateLength(vin)) {
        return false;
    }

    var check = vin[8]

    if (!(char.IsDigit(check) || check == 'X')) {
        return false;
    }

    return true;
}


function validateYearChar(vin) {
    if (!validateLength(vin)) {
        return false;
    }
    if (!validateCheckDigit(vin)) {
        return false;
    }
    for (var i = 0; i < 17; i++) {
        if (!yearValues.hasOwnProperty('name')) {
            return false;
        }

    }

    return true;
    
}

function validateChars(vin) {
    if (!validateLength(vin)) {
        return false;
    }
    if (!validateCheckDigit(vin)) {
        return false;
    }
    if (!validateYearChar(vin)) {
        return false;
    }
    for (var i = 0; i < 17; i++) {
        if (!replaceValues.hasOwnProperty('name')) {
            return false;
        }

    }
    return true;
}

function validateVin(vin) {
    if (!validateLength(vin)) {
        return false;
    }
    if (!validateCheckDigit(vin)) {
        return false;
    }
    if (!validateYearChar(vin)) {
        return false;
    }
    if (!validateChars(vin)) {
        return false;
    }
    var check = vin[8]

    var checkValue = (check != 'X' ? (parseInt(check)) : 10);

    var summed = 0;
    for (var i = 0; i < 17; i++) {
        summed += (replaceValues[vin.charAt(i)] * weights[i]);
    }

    return (summed % 11 === checkValue);

}


$.validator.addMethod('vinlength', function (value, element, params) {
    return validateLength(value);
});

$.validator.unobtrusive.adapters.add('vinlength', [], function (options) {

    options.messages['vinlength'] = options.message;
});

$.validator.addMethod('vincheckdigit', function (value, element, params) {
    return validateCheckDigit(value);
});

$.validator.unobtrusive.adapters.add('vinlength', [], function (options) {

    options.messages['vincheckdigit'] = options.message;
});


$.validator.addMethod('vinchars', function (value, element, params) {
    return validateChars(value);
});

$.validator.unobtrusive.adapters.add('vinchars', [], function (options) {

    options.messages['vinchars'] = options.message;
});




$.validator.addMethod('vinyear', function (value, element, params) {
    return validateYearChar(value);
});

$.validator.unobtrusive.adapters.add('vinyear', [], function (options) {

    options.messages['vinyear'] = options.message;
});


$.validator.addMethod('vinvalidation', function (value, element, params) {
    return validateVin(value);
});

$.validator.unobtrusive.adapters.add('vinvalidation', [], function (options) {

    options.messages['vinvalidation'] = options.message;
});
