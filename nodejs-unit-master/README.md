# NodeJS Unit

[![Nuget Package](https://flat.badgen.net/nuget/v/Adeptik.NodeJsUnit)](https://www.nuget.org/packages/Adeptik.NodeJsUnit/)

Simple test adapter between xUnit and Jasmine. Generates xUnit test Fact which runs tests with Jasmine.

## Installation

Install nuget package to your project:
```
Install-Package xunit -Version 2.4.1
```

## Getting started

1. Add a .js or .ts file to your project with simple test.

JavaScript:
```js
describe("Simple test (js)", function() {
    it("should be true", function() {
        expect(true).toBe(true);
    })
});
```

TypeScript:
```ts
import 'jasmine';

describe("Simple test (ts)", () => {
    it("should be true", () => {
        expect(true).toBe(true);
    })
});
```

2. In your project file '.csproj' add:
```xml
<ItemGroup>
    <NodeJsUnitTest Include="tests/**/*.ts;tests/**/*.js" />
</ItemGroup>
```

`NodeJsUnitTest` item type determines which files contain a jasmine tests.

3. Add package.json file to your project.

```json
{
  "name": "sample",
  "version": "0.1.0",
  "license": "MIT",
  "devDependencies": {
    "jasmine": "3.4.0"
  }
}
```

If tests is written in TypeScript, add additional dependencies to package.json
```json
{
  "devDependencies": {
    "@types/jasmine": "3.4.0",
    "ts-node": "8.3.0",
    "typescript": "3.5.3"
  }
}
```
4. Restore packages.

## Available settings

You can customize behaviour of generation test class by setting following properties in your .csproj file.

| Property name                        | Default value | Description                        |
|--------------------------------------|---------------|------------------------------------|
| NodeJsUnitTestGeneratedTestNamespace | NodeJsUnit    | Namespace for generated test class |
| NodeJsUnitTestGeneratedTestClassName | NodeJsTests   | Generated test class name          |
| NodeJsTestBaseName                   | NodeJsTest    | Generated test method name         |