using System.ComponentModel.DataAnnotations;

namespace ModernPhysics.Web.Areas.Admin.Pages.Manage.Categories
{
    public class InputCategoryModel
    {
        [Display(Name = "Nazwa *", Prompt = "Nazwa Kategorii")]
        public string Name { get; set; }

        [Display(Name = "Przyjazna nazwa", Prompt = "Nazwa-Kategorii (Opcjonalne)")]
        public string FriendlyName { get; set; }

        [Display(Name = "Ikona", Prompt = "fas fa-book (Opcjonalne)")]
        public string Icon { get; set; }
    }
}