/*
    Reference data: [dbo].[InvUser].

    Passwords are the SHA-512 hashes the API compares against; the plaintext is
    noted only because these are shared bootstrap accounts from DB_Init. Change
    them in any environment that is not a throwaway dev box.

    Matched on [Username], so an existing row keeps its generated [UserCode]
    (rows elsewhere reference it). Idempotent.
*/
IF '$(SeedReferenceData)' <> '0'
BEGIN
    PRINT '  -> Seeding [dbo].[InvUser]';

    MERGE INTO [dbo].[InvUser] AS T
    USING (VALUES
        -- PassWord@123$
        (N'NSAdmin', N'd04aa783775fa330e0ef7b78b329aa02b9ff004d2ae0ab39e2e0caa84a6468bc80c7316d43d62efd1f200a433242eb858e7fb3c0a447906deacf9ef3de3ddc2c', N'Namma Socials Admin', 1),
        -- AnandSiva@123$
        (N'Anand',   N'ce5fcfa41b61a411c438dee277898af452f2973e28352df79a0f80f0ae42aa9c0caf0ee85a6e2d63f91f7f329cb9ba98cddb27629137b47f8fe917b506cfb8f8', N'Krg Admin',           2)
    ) AS S ([Username], [Password], [FullName], [UserType])
        ON T.[Username] = S.[Username]
    WHEN NOT MATCHED BY TARGET THEN
        INSERT ([Username], [Password], [FullName], [UserType])
        VALUES (S.[Username], S.[Password], S.[FullName], S.[UserType]);
END
