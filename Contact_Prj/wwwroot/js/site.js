//https://www.codeproject.com/Articles/996400/WebGrid-Inline-Edit-and-Delete-of-data-in-ASP-NET
//https://www.codeproject.com/Articles/1110431/Gridview-in-ASP-NET-MVC
//https://stackoverflow.com/questions/62248536/inline-editing-of-textarea-with-jquery-inline-edit-js-get-the-id-and-save

document.addEventListener('DOMContentLoaded', () => {
    const saveButtons = document.querySelectorAll('.save-btn');
    const deleteButtons = document.querySelectorAll('.delete-btn');

    //SAVE INLINE button
    saveButtons.forEach(button => {
        button.addEventListener('click', async () => {
            const row = button.closest('tr');
            const id = row.dataset.id;
            const name = row.querySelector('td[data-name]').innerText.trim();
            const dateOfBirth = row.querySelector('td[data-dateofbirth]').innerText.trim();
            const married = row.querySelector('td[data-married]').innerText.trim() === 'true';
            const phone = row.querySelector('td[data-phone]').innerText.trim();
            const salary = parseFloat(row.querySelector('td[data-salary]').innerText.trim());

            const formData = new URLSearchParams();
            formData.append('id', id);
            formData.append('name', name);
            formData.append('dateOfBirth', dateOfBirth);
            formData.append('married', married);
            formData.append('phone', phone);
            formData.append('salary', salary);

            try {
                const response = await fetch(`/Contacts/Edit/${id}`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/x-www-form-urlencoded',
                        'RequestVerificationToken': document.querySelector('[name=__RequestVerificationToken]').value
                    },
                    body: formData.toString()
                });

                if (response.ok) {
                    alert('Contact updated successfully!');
                } else {
                    alert('Failed to update contact.');
                }
            } catch (error) {
                console.error('Error:', error);
            }
        });
    });

    //DELETE button
    deleteButtons.forEach(button => {
        button.addEventListener('click', async () => {
            const id = button.dataset.id;

            if (confirm('Are you sure you want to delete this contact?')) {
                try {
                    const response = await fetch(`/Contacts/Delete/${id}`, {
                        method: 'POST',
                        headers: {
                            'RequestVerificationToken': document.querySelector('[name=__RequestVerificationToken]').value
                        }
                    });

                    if (response.ok) {
                        alert('Contact deleted successfully!');
                        button.closest('tr').remove();
                    } else {
                        alert('Failed to delete contact.');
                    }
                } catch (error) {
                    console.error('Error:', error);
                }
            }
        });
    });
});
