const table = document.querySelector('#contactsTable');
const headers = table.querySelectorAll('th');
const sortDirections = new Map();

headers.forEach((header, index) => {
    header.addEventListener('click', () => {
        sortTable(index);
    });
});

//FILTER By Columns
function sortTable(columnIndex) {
    const rows = Array.from(table.querySelectorAll('tbody tr'));

    let sortDirection = sortDirections.get(columnIndex) || 'asc';

    sortDirection = sortDirection === 'asc' ? 'desc' : 'asc';
    sortDirections.set(columnIndex, sortDirection);

    rows.sort((a, b) => {
        const aValue = a.querySelectorAll('td')[columnIndex].textContent.trim();
        const bValue = b.querySelectorAll('td')[columnIndex].textContent.trim();

        if (sortDirection === 'asc') {
            return aValue.localeCompare(bValue);
        } else {
            return bValue.localeCompare(aValue);
        }
    });

    rows.forEach(row => table.querySelector('tbody').appendChild(row));
}

//FILTER by Input
const filterInput = document.querySelector('#filterInput');
filterInput.addEventListener('input', () => {
    const filterValue = filterInput.value.toLowerCase();
    const rows = table.querySelectorAll('tbody tr');
    rows.forEach(row => {
        const cells = row.querySelectorAll('td');
        let visible = false;
        for (const cell of cells) {
            if (cell.textContent.toLowerCase().includes(filterValue)) {
                visible = true;
                break;
            }
        }
        row.style.display = visible ? '' : 'none';
    });
});