// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


        function deleteItem({url, id, button, csrfToken, rowSelector = 'tr'}) {
            Swal.fire({
                title: 'Êtes-vous sûr ?',
                text: "Cette action est irréversible !",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#dc3545',
                cancelButtonColor: '#6c757d',
                confirmButtonText: 'Oui, supprimer',
                cancelButtonText: 'Annuler'
            }).then((result) => {
                if (result.isConfirmed) {
                    Swal.fire({
                        title: 'Suppression...',
                        text: 'Veuillez patienter',
                        didOpen: () => Swal.showLoading(),
                        allowOutsideClick: false,
                        allowEscapeKey: false
                    });

                    fetch(url, {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json',
                            'RequestVerificationToken': csrfToken
                        },
                        body: JSON.stringify({ id: id })
                    })
                        .then(response => response.json())
                        .then(data => {
                            if (data.success) {
                                let row = $(button).closest(rowSelector);
                                row.css('background-color', '#f8d7da'); // optional fade color
                                row.fadeOut(500, () => row.remove());

                                Swal.fire('Supprimé !', data.message || 'Suppression réussie.', 'success');
                            } else {
                                Swal.fire('Erreur', data.message || 'Une erreur est survenue.', 'error');
                            }
                        })
                        .catch(() => {
                            Swal.fire('Erreur', 'Erreur du serveur.', 'error');
                        });
                }
            });
        }


