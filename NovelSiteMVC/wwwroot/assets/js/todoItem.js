var utilities = {
    taskInputId: "taskInput",
    taskDueDateId: "taskDueDate",
    todoItemIdPrefix: "todoItem",
    flexCheckIdPrefix: "flexCheck",
    editInputIdPrefix: "editInput",
    confirmEditIdPrefix: "confirmEdit",
    firstLiIdPrefix: "firstLi",
    secondLiIdPrefix: "secondLi",
    thirdLiIdPrefix: "thirdLi",
    ShowTodoItems: function () {
        $.ajax({
            url: "/Todo/GetItems",
            type: "GET",
            success: function (result) {
                //loop on items to add them
                    console.log(result.list[0].id, result.list[0].task, result.list[0].dueDate, result.list[0].status);
                for (let x = 0; x < result.list.length; x++) {
                    console.log(result.list[x].id, result.list[x].task, result.list[x].dueDate, result.list[x].status);
                    utilities.AppendTodoItem(result.list[x].id, result.list[x].task, result.list[x].dueDate, result.list[x].status);
                }
            },
            error: function (xhr, status, error) {
                console.error("Error:", xhr.responseText);
            }
        });
    },
    CreateTodoItem: function () {
        let task = document.getElementById(this.taskInputId);
        let dueDate = document.getElementById(this.taskDueDateId);
        if (task.value !== "") {
            $.ajax({
                url: "/Todo/Create",
                data: { "Task": task.value, "DueDate": dueDate.value },
                type: "POST",
                success: function (result) {
                    //Add new item to todo list
                    utilities.AppendTodoItem(result.id, task.value, dueDate.value, result.status);
                },
                error: function (xhr, status, error) {
                    console.error("Error:", xhr.responseText);
                }
            });
        }
    },
    EditTodoItem: function (id) {
        let editInput = document.getElementById(`${this.editInputIdPrefix}${id}`);
        let confirmElement = document.getElementById(`${this.confirmEditIdPrefix}${id}`);
        editInput.removeAttribute("disabled");
        editInput.setAttribute("class", "form-control lead fw-normal mb-0 border-1 bg-transparent")
        confirmElement.removeAttribute("hidden");
    },
    ConfirmEditItem: function (id) {
        let input = document.getElementById(`${this.editInputIdPrefix}${id}`);
        let confirmElement = document.getElementById(`${this.confirmEditIdPrefix}${id}`);
        console.log("ConfirmEditItem entered, id: " + id);
        $.ajax({
            url: `/Todo/EditTask/${id}`,
            data: { "Task": input.value },
            type: "POST",
            success: function (result) {
                console.log("ConfirmEditItem success");
                input.setAttribute("disabled", "true");
                input.setAttribute("class", "form-control lead fw-normal mb-0 bg-transparent")
                confirmElement.setAttribute("hidden", "true");
            },
            error: function (xhr, status, error) {
                console.error("Error: ", xhr.responseText);
            }
        });  
    },
    DeleteTodoItem: function (id) {       
        $.ajax({
            url: `/Todo/Delete`,
            data: { "id": id },
            type: "POST",
            success: function (result) {
                //delete an item from todo list
                utilities.removeTodoItem(`${utilities.todoItemIdPrefix}${id}`);
            },
            error: function (xhr, status, error) {
                console.error("Error: ", xhr.responseText);
            }
        });        
    },
    CompleteTask: function (id) {
        let editInput = document.getElementById(`${this.editInputIdPrefix}${id}`)
        let firstLi = document.getElementById(`${this.firstLiIdPrefix}${id}`);
        let todoItem = document.getElementById(`${this.todoItemIdPrefix}${id}`);
        let color = "secondary";

        $.ajax({
            url: `/Todo/EditStatus`,
            data: { "id": id, "status": 2 },
            type: "POST",
            success: function (result) {
                //Edit the item
                editInput.style.textDecorationLine = "line-through";
                firstLi.className = `list-group-item d-flex align-items-center ps-0 pe-3 py-1 rounded-0 border-0 bg-${color}`;
                todoItem.className = `my-1 py-1 px-4 rounded-pill border border-3 border-${color} list-group list-group-horizontal bg-${color}`;
            },
            error: function (xhr, status, error) {
                console.error("Error: ", xhr.responseText);
            }
        });
    },
    CancelTaskComplete: function (id) {
        let editInput = document.getElementById(`${this.editInputIdPrefix}${id}`)
        let firstLi = document.getElementById(`${this.firstLiIdPrefix}${id}`);
        let todoItem = document.getElementById(`${this.todoItemIdPrefix}${id}`);
        let color = "success";

        $.ajax({
            url: `/Todo/EditStatus`,
            data: { "id": id, "status": 0 },
            type: "POST",
            success: function (result) {
                //Edit the item
                editInput.style.textDecorationLine = "none";
                firstLi.className = `list-group-item d-flex align-items-center ps-0 pe-3 py-1 rounded-0 border-0 bg-${color}`;
                todoItem.className = `my-1 py-1 px-4 rounded-pill border border-3 border-${color} list-group list-group-horizontal bg-${color}`;
            },
            error: function (xhr, status, error) {
                console.error("Error: ", xhr.responseText);
            }
        });
    },
    AppendTodoItem: function (id, task, dueDate, status) {
        console.log("append entered");
        console.log(status);
        let containerDiv = document.getElementById('todoList');
        let item = document.createElement("ul");
        let color;
        switch (status) {
            case 0:
                color = "success";
                break;
            case 1:
                color = "info";
                break;
            case 2:
                color = "secondary";
                break;
            case 3:
                color = "danger";
                break;
            default:
                color = "transparent";
                break;
        }
        item.className = `my-1 py-1 px-4 rounded-pill border border-3 border-${color} list-group list-group-horizontal bg-${color}`;
        item.id = `${this.todoItemIdPrefix}${id}`;
        item.innerHTML = `<li id="${this.firstLiIdPrefix}${id}" class="list-group-item d-flex align-items-center ps-0 pe-3 py-1 rounded-0 border-0 bg-${color}">
                        <div class="form-check">
                            <input onchange="ChangeTaskStatus_Clicked(${id})" class="form-check-input me-0 text-light" type="checkbox" for="${this.editInputIdPrefix}${id}" value="" id="${this.flexCheckIdPrefix}${id}"
                                    aria-label="..."/>
                        </div>
                    </li>
                    <li id="${this.secondLiIdPrefix}${id}" class="list-group-item ps-1 py-1 d-flex align-items-center flex-grow-1 border-0">
                        <input type="text" id="${this.editInputIdPrefix}${id}" name="${this.editInputIdPrefix}${id}" disabled class="form-control lead fw-normal mb-0 border-0" value="${task}" />
                    </li>
                    <li id="${this.thirdLiIdPrefix}${id}" class="list-group-item pe-2 py-1 rounded-0 border-0 bg-light">
                        <div class="d-flex flex-row justify-content-end mb-1">
                            <a id="${this.confirmEditIdPrefix}${id}" hidden href="#" onclick="confirmEdit_Clicked(${id});" class="text-success" data-mdb-tooltip-init title="اعتماد التعديل">
                                <i class="fas fa-check-square me-3"></i>
                            </a>
                            <a href="#!" onclick="editItem_Clicked(${id});" class="text-info" data-mdb-tooltip-init title="تعديل">
                                <i class="fas fa-pencil-alt me-3"></i>
                            </a>
                            <a href="#" onclick="deleteItem_Clicked(${id});" class="text-danger" data-mdb-tooltip-init title="إهمال">
                                <i class="fas fa-trash-alt"></i>
                            </a>
                        </div>
                        <div class="text-muted">
                            <span class="text-dark" data-mdb-tooltip-init title="تاريخ المهمة">
                                <p class="small mb-0"><i class="fas fa-info-circle me-2"></i>${dueDate}</p>
                            </span>
                        </div>
                    </li>`;

        containerDiv.appendChild(item);
    },
    removeTodoItem: function (id){
        let todoItem = document.getElementById(id);
        todoItem.remove();
    }
};
