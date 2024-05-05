<svelte:head>
	<title>Ruuat</title>
	<meta name="description" content="Ruuat" />
</svelte:head>

<!-- Modal for adding new food -->
<div class="modal" style="display: {modalOpen ? 'block' : 'none'}">
    <div class="modal-content">
		<h2>Lisää ruoka</h2>    
        <form on:submit|preventDefault={handleAddFood}>
            <label>
                Nimi:
                <input type="text" bind:value="{newFood.name}" />
            </label>
			<label>
                Kcal:
                <input type="number" bind:value="{newFood.calories}" />
            </label>
            <label>
                Hiilihydraatit:
                <input type="number" bind:value="{newFood.carbohydrates}" />
            </label>
            <label>
                Proteiini:
                <input type="number" bind:value="{newFood.protein}" />
            </label>
            <label>
                Rasva:
                <input type="number" bind:value="{newFood.fat}" />
            </label>
            <label>
                Kuitu:
                <input type="number" bind:value="{newFood.fiber}" />
            </label>
			<label>
                Hinta(€):
                <input type="number" step="0.01" bind:value="{newFood.price}" />
            </label>
            <button type="submit">Lisää</button>
			<button type="button" on:click={closeModal}>Peruuta</button>
        </form>
    </div>
</div>

<script>
	import { onMount } from "svelte";
	import { writable } from 'svelte/store';
	let foods = writable([]);
	let modalOpen = false;
    let newFood = { name: '', price: 0, calories: 0, carbohydrates: 0, protein: 0, fat: 0, fiber: 0};

	const fetchFoods = async () => {
		try {
			const response = await fetch('http://localhost:5126/api/Foods');
			if (!response.ok) {
				throw new Error('Failed to fetch foods from the API');
			}
			const data = await response.json();
			foods.set(data);
		} catch (error) {
			console.error(error);
		}
	}

	const handleAddFood = async () => {
        try {
			const id = Date.now(); //Spaghettisolution, could be improved
			newFood.id = id;

            const response = await fetch('http://localhost:5126/api/Foods', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(newFood)
            });
            if (!response.ok) {
                throw new Error('Failed to add new food item');
            }
            // Refresh the list of foods after adding a new item
            fetchFoods();
            // Close the modal
            modalOpen = false;
            // Reset the newFood object
            newFood = { name: '', carbohydrates: 0, protein: 0, fat: 0, fiber: 0 };
        } catch (error) {
            console.error(error);
        }
    };
	const closeModal = () => {
        modalOpen = false; // Set modalOpen to false to close the modal
    };

	const handleDelete = async (id) => {
        try {
            const response = await fetch(`http://localhost:5126/api/Foods/${id}`, {
                method: 'DELETE'
            });
            if (!response.ok) {
                throw new Error('Failed to delete food item');
            }
            // Remove the deleted item from the array
            foods.update(items => items.filter(item => item.id !== id));
        } catch (error) {
            console.error(error);
        }
    };

	onMount(fetchFoods);
</script>

<section>
	<h1>
		Ruuat
	</h1>
	
		<div class="titles-container">
			<p class="title">Ruoka</p>
			<p class="title">Ravinteet per 100g</p>
		</div>
		
		<!-- Table displaying food contents -->
		<table>
			<thead>
				<tr>
					<th></th> <!-- Jätetään checkbox ja nimi th tyhjäksi -->
					<th></th> <!-- Jätetään checkbox ja nimi th tyhjäksi -->
					<th style="padding-left: 10px;">Kcal</th>
					<th>Hiil</th>
					<th>Prot</th>
					<th>Rasva</th>
					<th>Kuitu</th>
				</tr>
			</thead>
			<tbody>
				{#each $foods as food}
					<tr>
						<td style="padding-right: 10px; text-align: center;"><input type="checkbox" /></td> <!-- Checkbox -->
						<td>{food.name}</td> 
						<td style="padding-left: 10px;">{food.calories}</td> 
						<td>{food.carbohydrates}</td> 
						<td>{food.protein}</td> 
						<td>{food.fat}</td> 
						<td>{food.fiber}</td>
						<td>
							<button class="delete-button" on:click={() => handleDelete(food.id)}>
								<svg viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
									<circle cx="12" cy="12" r="10" stroke="currentColor" stroke-width="2" />
									<line x1="6" y1="12" x2="18" y2="12" stroke="currentColor" stroke-width="2" />
								</svg>
							</button>
						</td>
					</tr>
				{/each}
			</tbody>
		</table>
	

	<button class="calc-button" on:click={() => modalOpen = true}>
		<span>Lisää ruoka</span>
		<span class="plus">+</span>
	</button>
</section>
	
<style>
	section {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		flex: 0.6;
		
	}
	

	h1 {
		width: 100%;
	}
	.titles-container {
        display: flex;
        justify-content: center;
        width: 100%;
    }

    .title {
        text-align: center;
		
    }
	.titles-container p {
		margin: 0 55px;
	}

	p {
		margin: 0 10px;
	}
	.calc-button {
		display: flex;
		justify-content: space-between;
		align-items: center;
        padding: 10px 20px;
		cursor: pointer;
        
		width: 300px;  
         
    }
	.plus {
        margin-left: auto;
    }

	.delete-button {
        background-color: transparent;
        border: none;
        cursor: pointer;
        padding: 0;
    }

    .delete-button svg {
        width: 24px;
        height: 24px;
        fill: none;
        stroke: #000;
        stroke-linecap: round;
        stroke-linejoin: round;
        stroke-width: 2;
    }
	
    .modal {
        display: none; /* Hidden by default */
        position: fixed; /* Stay in place */
        z-index: 1; /* Sit on top */
        left: 0;
        top: 0;
        width: 100%; 
        height: 100%; 
        overflow: auto; /* Enable scroll if needed */
        background-color: rgb(0,0,0); 
        background-color: rgba(0,0,0,0.4); 
    }

    .modal-content {
        background-color: #fefefe;
        margin: 15% auto; 
        padding: 20px;
        border: 1px solid #888;
        width: 80%; /* Could be more or less, depending on screen size */
    }
	.modal input {
		width: 6%;
        margin-right: 10px; /* Add some margin between inputs */
        margin-bottom: 5px; /* Add some bottom margin to separate the inputs */
	}
	.modal button {
        width: 10%; 
        margin-top: 10px; 
    }
	.modal button[type="submit"] {
        background-color: green;
        color: white;
        border: 2px solid #333;
		cursor: pointer;
    }
	.modal button[type="button"] {
        background-color: red;
        color: white;
        border: 2px solid #333;
		cursor: pointer;
    }
	.modal h2 {
		text-align: center;
		text-decoration: underline;
		font-weight: bold;
	}
</style>

