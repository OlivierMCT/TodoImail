using Microsoft.AspNetCore.Components;
using TodoImail.BlazorApp.Client.Models;
using TodoImail.BlazorApp.Client.Services;

namespace TodoImail.BlazorApp.Client.Components.Pages {
    public partial class TodoList {
        [Inject] public IServiceProvider ServiceProvider { get; set; }
        public ITodoImailClientService? Service { get; set; }
        
        public List<Todo> Todos { get; set; } = [];
        public List<Category> Categories { get; set; } = [];
        public List<Category> SelectedCategories { get; set; } = [];

        private bool? _sorted;
        public bool? Sorted {
            get { return _sorted; }
            set {
                _sorted = value;
                Todos = (_sorted switch {
                    true => Todos.OrderBy(t => t.Label),
                    false => Todos.OrderByDescending(t => t.Label),
                    _ => Todos.OrderBy(t => t.Id)
                }).ToList();
            }
        }


        public bool IsLoading { get; set; } = true;

        protected override async Task OnAfterRenderAsync(bool firstRender) {
            if (firstRender) {
                Service = ServiceProvider.GetRequiredService<ITodoImailClientService>();
                Categories = await Service.GetCategories();
                Todos = await Service.GetTodos();
                IsLoading = false;
                StateHasChanged();
            }
        }

        private void ToggleSelectedCategory(Category category) {
            if(!SelectedCategories.Remove(category)) SelectedCategories.Add(category);
        }

        private bool IsSelectedCategory(Category? category) {
            return category is not null && SelectedCategories.Contains(category);
        }

        private void Sort() {
            Sorted = Sorted switch {
                true => false,
                false => null,
                _ => true
            };
        }

        private void Toggled(TodoDetail toggledTodo) {
            int index = Todos.FindIndex(t => t.Id == toggledTodo.Id);
            if (index >= 0) Todos[index] = toggledTodo;
        }

        private void Deleting(Todo deletingTodo) {
            int index = Todos.FindIndex(t => t.Id == deletingTodo.Id);
            if (index >= 0) Todos.RemoveAt(index);
        }
    }
}
