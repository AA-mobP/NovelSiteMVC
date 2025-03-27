var utilities = {
    CreateTodoItem: function () {
        let task = document.getElementById('taskInput');
        let dueDate = document.getElementById('taskDueDate');
        console.log("dueDate: " + dueDate.value);
        if (task.value !== "") {
            console.log(`task value: ${task.value}`);
            $.ajax({
                url: "/Todo/Create",
                data: { "Task": task.value, "DueDate": dueDate.value },//Need Confirmation
                type: "POST",
                success: function (result) {
                    console.log("CreateTodoItem success");
                    //Add new item to todo list
                    utilities.AppendTodoItem(task, dueDate);
                },
                error: function (xhr, status, error) {
                    console.error("Error:", xhr.responseText);
                }
            });
        }
    },
    EditTodoItem: function (id, confirmId) {
        console.log("enterd EditTodoItem");
        console.log("id: " + id);
        let editInput = document.getElementById(id);
        let confirmElement = document.getElementById(confirmId);
        console.log("edintInput: " + editInput);
        editInput.removeAttribute("disabled");
        editInput.setAttribute("class", "form-control lead fw-normal mb-0 border-1 bg-transparent")
        console.log("confirmId: " + confirmElement);
        confirmElement.removeAttribute("hidden");
        
    },
    DeleteTodoItem: function (htmlId) {
        let numId = htmlId.slice(8);
        console.log("DeleteTodoItem entered, id: " + numId);
        $.ajax({
            url: `/Todo/Delete`,
            data: { "id": numId },
            type: "POST",
            success: function (result) {
                console.log("DeleeTodoItem success");
                //delete an item from todo list
                utilities.removeTodoItem(htmlId);
            },
            error: function (xhr, status, error) {
                console.error("Error: ", xhr.responseText);
            }
        });        
    },
    AppendTodoItem: function (task, dueDate) {
        let containerDiv = document.getElementById('todoList');
        let itemPosition = containerDiv.childElementCount + 1;
        let item = document.createElement("ul");
        let color;
        if (!dueDate.value) {
            color = "success";
            dueDate.value = "Now";
        }
        else {
            color = "info";
        }
        item.className = `my-1 py-1 px-4 rounded-pill border border-3 border-${color} list-group list-group-horizontal bg-${color}`;
        item.innerHTML = `<li class="list-group-item d-flex align-items-center ps-0 pe-3 py-1 rounded-0 border-0 bg-${color}">
                        <div class="form-check">
                            <input class="form-check-input me-0" type="checkbox" for="${itemPosition}" value="" id="flexCheckChecked${itemPosition}"
                                    aria-label="..."/>
                        </div>
                    </li>
                    <li class="list-group-item ps-1 py-1 d-flex align-items-center flex-grow-1 border-0 bg-light">
                        <input type="text" name="${itemPosition}" disabled class="form-control lead fw-normal mb-0 border-0 bg-transparent" value="${task.value}" />
                    </li>
                    <li class="list-group-item pe-2 py-1 rounded-0 border-0 bg-light">
                        <div class="d-flex flex-row justify-content-end mb-1">
                            <a onclick="EditTodoItem();" class="text-info" data-mdb-tooltip-init title="Edit todo">
                                <i class="fas fa-pencil-alt me-3"></i>
                            </a>
                            <a onclick="DeleteTodoItem();" class="text-danger" data-mdb-tooltip-init title="Delete todo">
                                <i class="fas fa-trash-alt"></i>
                            </a>
                        </div>
                        <div class="text-end text-muted">
                            <a href="#!" class="text-muted" data-mdb-tooltip-init title="Due Date">
                                <p class="small mb-0"><i class="fas fa-info-circle me-2"></i>${dueDate.value}</p>
                            </a>
                        </div>
                    </li>`;

        containerDiv.appendChild(item);
    },
    removeTodoItem: function (id){
        let todoItem = document.getElementById(id);
        todoItem.remove();
        console.log("removeTodoItem done");
    }
};
