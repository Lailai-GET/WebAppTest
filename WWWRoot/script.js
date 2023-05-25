getData();

function updateViewInMemoryDb() {
    document.getElementById("app").innerHTML = /*HTML*/`
        <button onclick="changeDirectory('sqlDatabase')">
            Bruk SQL-databasen 
        </button>
        <button onclick="changeDirectory('inMemoryDb')">
            Bruk InMemory-databasen 
        </button>
        <table>
            <tr>
                <th>
                    GUID
                </th>
                <th>
                    Name
                </th>
                <th>
                    Description
                </th>
            </tr>
            ${generateItemHTML()}
        </table>
        <div>
            nytt navn: 
            <input type="text" value="${model.inputName}" onchange="model.inputName = this.value"/>
            ny beskrivelse: 
            <input type="text" value="${model.inputDescription}" onchange="model.inputDescription = this.value" />

            <button onclick="createNewItem()">
                Legg til ny
            </button>
        </div>
    `;

}


function generateItemHTML() {

    return model.items.map(item =>
        `
        <tr>
            <td>
                ${item.id}
            </td>
            <td>
                ${item.name}
            </td>
            <td>
                ${item.description}
            </td>
            <td>
                <button onclick="editItem('${item.id}')"> endre </button> 
                <button onclick="deleteItem('${item.id}')"> fjern </button>
            </td>
        </tr > 
        `
    ).join("");
}

function editItem(itemId) {
    let itemToEdit = model.items.find(item => item.id === itemId);
    model.nameEdit = itemToEdit.name;
    model.descriptionEdit = itemToEdit.description;
    document.getElementById("app").innerHTML += `
    <div>
            nytt navn:
            <input type="text" value="${itemToEdit.name}" onchange="model.nameEdit = this.value"/>
            ny beskrivelse: 
            <input type="text" value="${itemToEdit.description}" onchange="model.descriptionEdit = this.value" />
            <button onclick="saveEdit('${itemId}')">
                Lagre endringer
            </button>
        </div>
    `;
}

async function getData() {
    const response = await axios.get(model.directory);
    model.items = response.data;
    updateViewInMemoryDb();
}

async function createNewItem() {
    let newItem = {
        name: model.inputName,
        description: model.inputDescription
    }
    const response = await axios.post(model.directory, newItem);
    model.inputName = "";
    model.inputDescription = "";
    await getData();
}

async function saveEdit(itemId) {
    let editItem = {
        id: itemId,
        name: model.nameEdit,
        description: model.descriptionEdit
    }
    const response = await axios.put(model.directory, editItem);
    await getData();
}

async function deleteItem(itemId) {
    const response = await axios.delete(model.directory + "/" + itemId);
    await getData();
}

async function changeDirectory(changeTothis) {
    if (changeTothis === "sqlDatabase") {
        model.directory = "/sqlTest";
    }
    if (changeTothis === "inMemoryDb") {
        model.directory = "/test";
    }
    await getData();
}