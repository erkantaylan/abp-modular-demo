namespace LayeredDemo.Permissions;

public static class LayeredDemoPermissions
{
    public const string GroupName = "LayeredDemo";


    
    public static class Todos
    {
        public const string Default = GroupName + ".Todos";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }
}
