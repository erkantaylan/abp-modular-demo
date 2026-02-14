namespace AngularDemo.Permissions;

public static class AngularDemoPermissions
{
    public const string GroupName = "AngularDemo";

    public static class Todos
    {
        public const string Default = GroupName + ".Todos";
        public const string Create = Default + ".Create";
        public const string Complete = Default + ".Complete";
    }
}
