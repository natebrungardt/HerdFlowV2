using HerdFlow.Api.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HerdFlow.Api.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20260401120000_EnableSupabaseRls")]
    public class EnableSupabaseRls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                DO $$
                BEGIN
                    IF NOT EXISTS (SELECT 1 FROM pg_roles WHERE rolname = 'authenticated') THEN
                        RAISE NOTICE 'Skipping Supabase RLS migration because role "authenticated" does not exist.';
                        RETURN;
                    END IF;

                    IF NOT EXISTS (
                        SELECT 1
                        FROM pg_proc p
                        JOIN pg_namespace n ON n.oid = p.pronamespace
                        WHERE n.nspname = 'auth'
                          AND p.proname = 'uid'
                    ) THEN
                        RAISE NOTICE 'Skipping Supabase RLS migration because function auth.uid() does not exist.';
                        RETURN;
                    END IF;

                    ALTER TABLE public."__EFMigrationsHistory" ENABLE ROW LEVEL SECURITY;
                    ALTER TABLE public."ActivityLogEntries" ENABLE ROW LEVEL SECURITY;
                    ALTER TABLE public."Notes" ENABLE ROW LEVEL SECURITY;
                    ALTER TABLE public."Cows" ENABLE ROW LEVEL SECURITY;
                    ALTER TABLE public."WorkdayCows" ENABLE ROW LEVEL SECURITY;
                    ALTER TABLE public."Workdays" ENABLE ROW LEVEL SECURITY;
                    ALTER TABLE public."WorkdayNotes" ENABLE ROW LEVEL SECURITY;

                    CREATE POLICY "ActivityLogEntries select own"
                    ON public."ActivityLogEntries"
                    FOR SELECT
                    TO authenticated
                    USING ("UserId" = (SELECT auth.uid())::text);

                    CREATE POLICY "ActivityLogEntries insert own"
                    ON public."ActivityLogEntries"
                    FOR INSERT
                    TO authenticated
                    WITH CHECK ("UserId" = (SELECT auth.uid())::text);

                    CREATE POLICY "ActivityLogEntries update own"
                    ON public."ActivityLogEntries"
                    FOR UPDATE
                    TO authenticated
                    USING ("UserId" = (SELECT auth.uid())::text)
                    WITH CHECK ("UserId" = (SELECT auth.uid())::text);

                    CREATE POLICY "ActivityLogEntries delete own"
                    ON public."ActivityLogEntries"
                    FOR DELETE
                    TO authenticated
                    USING ("UserId" = (SELECT auth.uid())::text);

                    CREATE POLICY "Notes select own"
                    ON public."Notes"
                    FOR SELECT
                    TO authenticated
                    USING ("UserId" = (SELECT auth.uid())::text);

                    CREATE POLICY "Notes insert own"
                    ON public."Notes"
                    FOR INSERT
                    TO authenticated
                    WITH CHECK ("UserId" = (SELECT auth.uid())::text);

                    CREATE POLICY "Notes update own"
                    ON public."Notes"
                    FOR UPDATE
                    TO authenticated
                    USING ("UserId" = (SELECT auth.uid())::text)
                    WITH CHECK ("UserId" = (SELECT auth.uid())::text);

                    CREATE POLICY "Notes delete own"
                    ON public."Notes"
                    FOR DELETE
                    TO authenticated
                    USING ("UserId" = (SELECT auth.uid())::text);

                    CREATE POLICY "Cows select own"
                    ON public."Cows"
                    FOR SELECT
                    TO authenticated
                    USING ("UserId" = (SELECT auth.uid())::text);

                    CREATE POLICY "Cows insert own"
                    ON public."Cows"
                    FOR INSERT
                    TO authenticated
                    WITH CHECK ("UserId" = (SELECT auth.uid())::text);

                    CREATE POLICY "Cows update own"
                    ON public."Cows"
                    FOR UPDATE
                    TO authenticated
                    USING ("UserId" = (SELECT auth.uid())::text)
                    WITH CHECK ("UserId" = (SELECT auth.uid())::text);

                    CREATE POLICY "Cows delete own"
                    ON public."Cows"
                    FOR DELETE
                    TO authenticated
                    USING ("UserId" = (SELECT auth.uid())::text);

                    CREATE POLICY "Workdays select own"
                    ON public."Workdays"
                    FOR SELECT
                    TO authenticated
                    USING ("UserId" = (SELECT auth.uid())::text);

                    CREATE POLICY "Workdays insert own"
                    ON public."Workdays"
                    FOR INSERT
                    TO authenticated
                    WITH CHECK ("UserId" = (SELECT auth.uid())::text);

                    CREATE POLICY "Workdays update own"
                    ON public."Workdays"
                    FOR UPDATE
                    TO authenticated
                    USING ("UserId" = (SELECT auth.uid())::text)
                    WITH CHECK ("UserId" = (SELECT auth.uid())::text);

                    CREATE POLICY "Workdays delete own"
                    ON public."Workdays"
                    FOR DELETE
                    TO authenticated
                    USING ("UserId" = (SELECT auth.uid())::text);

                    CREATE POLICY "WorkdayNotes select own"
                    ON public."WorkdayNotes"
                    FOR SELECT
                    TO authenticated
                    USING ("UserId" = (SELECT auth.uid())::text);

                    CREATE POLICY "WorkdayNotes insert own"
                    ON public."WorkdayNotes"
                    FOR INSERT
                    TO authenticated
                    WITH CHECK ("UserId" = (SELECT auth.uid())::text);

                    CREATE POLICY "WorkdayNotes update own"
                    ON public."WorkdayNotes"
                    FOR UPDATE
                    TO authenticated
                    USING ("UserId" = (SELECT auth.uid())::text)
                    WITH CHECK ("UserId" = (SELECT auth.uid())::text);

                    CREATE POLICY "WorkdayNotes delete own"
                    ON public."WorkdayNotes"
                    FOR DELETE
                    TO authenticated
                    USING ("UserId" = (SELECT auth.uid())::text);

                    CREATE POLICY "WorkdayCows select own"
                    ON public."WorkdayCows"
                    FOR SELECT
                    TO authenticated
                    USING (
                        EXISTS (
                            SELECT 1
                            FROM public."Workdays" w
                            WHERE w."Id" = "WorkdayId"
                              AND w."UserId" = (SELECT auth.uid())::text
                        )
                    );

                    CREATE POLICY "WorkdayCows insert own"
                    ON public."WorkdayCows"
                    FOR INSERT
                    TO authenticated
                    WITH CHECK (
                        EXISTS (
                            SELECT 1
                            FROM public."Workdays" w
                            WHERE w."Id" = "WorkdayId"
                              AND w."UserId" = (SELECT auth.uid())::text
                        )
                        AND EXISTS (
                            SELECT 1
                            FROM public."Cows" c
                            WHERE c."Id" = "CowId"
                              AND c."UserId" = (SELECT auth.uid())::text
                        )
                    );

                    CREATE POLICY "WorkdayCows update own"
                    ON public."WorkdayCows"
                    FOR UPDATE
                    TO authenticated
                    USING (
                        EXISTS (
                            SELECT 1
                            FROM public."Workdays" w
                            WHERE w."Id" = "WorkdayId"
                              AND w."UserId" = (SELECT auth.uid())::text
                        )
                    )
                    WITH CHECK (
                        EXISTS (
                            SELECT 1
                            FROM public."Workdays" w
                            WHERE w."Id" = "WorkdayId"
                              AND w."UserId" = (SELECT auth.uid())::text
                        )
                        AND EXISTS (
                            SELECT 1
                            FROM public."Cows" c
                            WHERE c."Id" = "CowId"
                              AND c."UserId" = (SELECT auth.uid())::text
                        )
                    );

                    CREATE POLICY "WorkdayCows delete own"
                    ON public."WorkdayCows"
                    FOR DELETE
                    TO authenticated
                    USING (
                        EXISTS (
                            SELECT 1
                            FROM public."Workdays" w
                            WHERE w."Id" = "WorkdayId"
                              AND w."UserId" = (SELECT auth.uid())::text
                        )
                    );
                END
                $$;
                """);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                DROP POLICY IF EXISTS "WorkdayCows delete own" ON public."WorkdayCows";
                DROP POLICY IF EXISTS "WorkdayCows update own" ON public."WorkdayCows";
                DROP POLICY IF EXISTS "WorkdayCows insert own" ON public."WorkdayCows";
                DROP POLICY IF EXISTS "WorkdayCows select own" ON public."WorkdayCows";
                """);

            DropDirectUserPolicies(migrationBuilder, "ActivityLogEntries");
            DropDirectUserPolicies(migrationBuilder, "Notes");
            DropDirectUserPolicies(migrationBuilder, "Cows");
            DropDirectUserPolicies(migrationBuilder, "Workdays");
            DropDirectUserPolicies(migrationBuilder, "WorkdayNotes");

            migrationBuilder.Sql("""
                ALTER TABLE public."__EFMigrationsHistory" DISABLE ROW LEVEL SECURITY;
                ALTER TABLE public."ActivityLogEntries" DISABLE ROW LEVEL SECURITY;
                ALTER TABLE public."Notes" DISABLE ROW LEVEL SECURITY;
                ALTER TABLE public."Cows" DISABLE ROW LEVEL SECURITY;
                ALTER TABLE public."WorkdayCows" DISABLE ROW LEVEL SECURITY;
                ALTER TABLE public."Workdays" DISABLE ROW LEVEL SECURITY;
                ALTER TABLE public."WorkdayNotes" DISABLE ROW LEVEL SECURITY;
                """);
        }

        private static void CreateDirectUserPolicies(MigrationBuilder migrationBuilder, string tableName)
        {
            migrationBuilder.Sql($"""
                CREATE POLICY "{tableName} select own"
                ON public."{tableName}"
                FOR SELECT
                TO authenticated
                USING ("UserId" = (SELECT auth.uid())::text);

                CREATE POLICY "{tableName} insert own"
                ON public."{tableName}"
                FOR INSERT
                TO authenticated
                WITH CHECK ("UserId" = (SELECT auth.uid())::text);

                CREATE POLICY "{tableName} update own"
                ON public."{tableName}"
                FOR UPDATE
                TO authenticated
                USING ("UserId" = (SELECT auth.uid())::text)
                WITH CHECK ("UserId" = (SELECT auth.uid())::text);

                CREATE POLICY "{tableName} delete own"
                ON public."{tableName}"
                FOR DELETE
                TO authenticated
                USING ("UserId" = (SELECT auth.uid())::text);
                """);
        }

        private static void DropDirectUserPolicies(MigrationBuilder migrationBuilder, string tableName)
        {
            migrationBuilder.Sql($"""
                DROP POLICY IF EXISTS "{tableName} delete own" ON public."{tableName}";
                DROP POLICY IF EXISTS "{tableName} update own" ON public."{tableName}";
                DROP POLICY IF EXISTS "{tableName} insert own" ON public."{tableName}";
                DROP POLICY IF EXISTS "{tableName} select own" ON public."{tableName}";
                """);
        }
    }
}
