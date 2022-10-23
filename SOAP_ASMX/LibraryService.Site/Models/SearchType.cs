using System.ComponentModel.DataAnnotations;

namespace LibraryService.Site.Models;

public enum SearchType
{
    [Display(Name = "Заголовок")]
    Title,
    [Display(Name = "Автор")]
    Author,
    [Display(Name = "Категория")]
    Category
}