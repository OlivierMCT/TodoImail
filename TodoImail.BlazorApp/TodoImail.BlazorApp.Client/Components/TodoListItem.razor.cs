using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using TodoImail.BlazorApp.Client.Models;
using TodoImail.BlazorApp.Client.Services;

namespace TodoImail.BlazorApp.Client.Components {
    public partial class TodoListItem {
        [Inject] public required IJSRuntime JavaScriptService { get; set; }
        [Inject] public required ITodoImailClientService Service { get; set; }

        [Parameter] public required Todo Todo { get; set; }
        public TodoDetail? TodoDetail { get; set; }

        [Parameter] public EventCallback<TodoDetail> OnToggled { get; set; }
        [Parameter] public EventCallback<Todo> OnDeleting { get; set; }


        public async Task LoadDetails() {
            if (TodoDetail is null)
                TodoDetail = await Service.GetTodo(Todo.Id);
            else
                TodoDetail = null;
        }

        public async Task Toggle() {
            var detail = await Service.ToggleTodo(Todo);
            _ = OnToggled.InvokeAsync(detail);
            if (TodoDetail is not null) TodoDetail = detail;
        }

        public async Task Remove() {
            try {
                await Service!.RemoveTodo(Todo);
                _ = OnDeleting.InvokeAsync(Todo);
            } catch (Exception ex) {
                _ = JavaScriptService.InvokeVoidAsync("alert", ex.Message);
            }
        }
    }
}
