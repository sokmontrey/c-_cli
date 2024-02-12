# Program Documentation

## Design decisions

## Naming conventions

All class and struct names are in PascalCase.

All method names are in camelCase.

All variable name are in snake_case.

The "_" before a variable name is used to indicate that the variable is private.
Example: _variable_name 

## Technical decisions

Since the price does not need to be precise, **float is used to store the price of the menu items**.

Dictionary is used to store thing such as all the available menu items and coupon codes for a better scalability and easier access to the menu items.

Struct is used to encapsulate simple states of the program as values rather than reference.
