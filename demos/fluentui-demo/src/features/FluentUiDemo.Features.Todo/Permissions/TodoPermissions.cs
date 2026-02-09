namespace FluentUiDemo.Permissions;

public static class TodoPermissions
{
    public const string GroupName = "FluentUiDemo.Todos";

    public static class Todos
    {
        public const string Default = GroupName;
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }
}
