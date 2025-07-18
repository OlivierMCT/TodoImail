using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using TodoImail.BlazorApp.Client.Models;
using TodoImail.BlazorApp.Client.Services;

namespace TodoImail.BlazorApp.Client.Components.Pages;
public partial class TodoAddForm {
    [Inject] public required NavigationManager Router { get; set; }
    [Inject] public required IJSRuntime JavaScriptService { get; set; }
    [Inject] public required IServiceProvider ServiceProvider { get; set; }
    public ITodoImailClientService? Service { get; set; }

    public List<Category> Categories { get; set; } = [];
    public bool IsCategoriesLoading { get; set; } = true;

    public bool ShowCustomeError { get; set; } = false;
    public bool IsSaving { get; set; } = false;

    public TodoAdd TodoModel { get; set; } = new();

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        if (firstRender) {
            Service = ServiceProvider.GetRequiredService<ITodoImailClientService>();
            Categories = await Service.GetCategories();
            Categories = [.. Categories.OrderBy(c => c.Label)];
            IsCategoriesLoading = false;
            StateHasChanged();
        }
    }

    public required EditForm TodoEditForm { get; set; }
    public async Task SaveTodo() {
        await Task.Delay(100); // Mise à jour de l'Html
        ShowCustomeError = !TodoEditForm.EditContext!.Validate();
        if (ShowCustomeError) return;

        IsSaving = true;
        StateHasChanged(); // Mise à jour de l'Html
        var t1 = Task.Delay(3500);
        var t2 = Service!.PostTodo(TodoModel);

        try {
            await Task.WhenAll(t1, t2);
            var todo = t2.Result;
            if(await JavaScriptService.InvokeAsync<bool>("confirm", $"la tâche n°{todo?.Id} a été enregistrée. Voulez en ajouter une autre ?")) {
                TodoModel = new();
            } else {
                Router.NavigateTo("/mes-taches");
            }
        } catch {
            ShowCustomeError = true;
        } finally {
            IsSaving = false;
        }
    }

    public void NotifyFieldChanged(string propertyName) {
        var fieldLongitude = TodoEditForm.EditContext!.Field(propertyName);
        TodoEditForm.EditContext!.NotifyFieldChanged(fieldLongitude);
    }


}
