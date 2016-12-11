# ef-core-bug-POC
My proof of concept for Entity Framework Core Tools or Designer bug.

[My question on StackOverflow](http://stackoverflow.com/questions/40667605/altering-column-with-index-on-it-in-entity-framework-core)

## FIX - update
This is fixed in EF Core 1.1.0 version.
But - during updating to EF Core 1.1.0 you have to add to `project.json` in tools node `"Microsoft.EntityFrameworkCore.Tools.DotNet": "1.1.0-preview4-final"`

So the final code in `projetc.json`:
```
  "tools": {
    "Microsoft.EntityFrameworkCore.Tools": "1.0.0-preview4-final",
    "Microsoft.EntityFrameworkCore.Tools.DotNet": "1.1.0-preview4-final"
  },
```

## Bug details
I've changed the existing column, which had index on it created few migrations earlier:

```
// ... other changes
migrationBuilder.CreateIndex(
    name: "IX_Interactions_OrganisationToId",
    table: "Interactions",
    column: "OrganisationToId");
```

I've changed it from optional to required (not-nullable). Entity Framework Core generated such a migration:

```
// ... other changes
migrationBuilder.AlterColumn<Guid>(
    name: "OrganisationToId",
    table: "Interactions",
    nullable: false);
```

But you cannot alter the column which has index on it. Unfortunately EF Core is not supporting this in case of changing field to non-nullable.

When renaming field - works like a charm and before rename simply drops index and restores it after alter.

## Workaround
At the moment I've modified the migration and dropped down the index before altering the column, and manually restore it after the operation.

It did the job.

```
// drop index before altering column
migrationBuilder.DropIndex(name: "IX_Interactions_OrganisationToId", table: "Interactions");

// actually altering column
migrationBuilder.AlterColumn<Guid>(
   name: "OrganisationToId",
   table: "Interactions",
   nullable: false);

// restore index
migrationBuilder.CreateIndex(
   name: "IX_Interactions_OrganisationToId",
   table: "Interactions",
   column: "OrganisationToId");
```