namespace FluentUiDemo.Permissions;

public static class ShoppingPermissions
{
    public const string GroupName = "FluentUiDemo.Shopping";

    public static class Products
    {
        public const string Default = GroupName;
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }
}
