<script>
    import { onMount } from 'svelte';
    import { writable } from 'svelte/store';
    let formData = {
        calories: 0,
        protein: 0,
        carbohydrates: 0,
        fat: 0,
        fiber: 0
    };

    let results = writable([]);
    let formSubmitted = false;

     // Function to handle checkbox change event
     function handleCheckboxChange(event) {
    const { name, checked } = event.target;
    if (checked) {
        const input = document.querySelector(`input[name="${name}"][type="number"]`);
        if (input && parseFloat(input.value) > 9999) {
            input.value = '9999'; // Set the value to the maximum allowed value
            formData[name] = 9999;
        } else {
            formData[name] = parseFloat(input.value) || 0;
        }
    } else {
        formData[name] = 0; // Set to 0 if unchecked
    }
}

    // Function to handle number input change event
    function handleInputChange(event) {
        const { name, value } = event.target;
        const checkbox = document.querySelector(`input[name="${name}"][type="checkbox"]`);
        if (checkbox && checkbox.checked) {
            const parsedValue = parseFloat(value);
        if (parsedValue > 9999) {
            event.target.value = '9999'; // Set the value to the maximum allowed value
            formData[name] = 9999;
        } else {
            formData[name] = parsedValue || 0;
        }
    }
    }

    // Function to handle form submission
    async function handleSubmit() {
        try {
            const anyCheckboxEmpty = Object.entries(formData).some(([key, value]) => {
                const checkbox = document.querySelector(`input[name="${key}"][type="checkbox"]`);
                if (checkbox && checkbox.checked && (!value || isNaN(value) || value <= 0)) {
                    return true;
                }
                return false;
            });

            if (anyCheckboxEmpty) {
                alert('Syötä arvo 0-9999 valituille ravinteille');
                return;
            }

            // Check if at least one checkbox is checked and its associated input field contains a valid value
            const anyValidCheckboxChecked = Object.entries(formData).some(([key, value]) => {
                const checkbox = document.querySelector(`input[name="${key}"][type="checkbox"]`);
                if (checkbox && checkbox.checked && (!isNaN(value) && value > 0)) {
                    return true;
                }
                return false;
            });
            if (!anyValidCheckboxChecked) {
            alert('Valitse ainakin yksi ravinne ja syötä sille arvo');
            return;
            }

            const response = await fetch('http://localhost:5126/api/Calculate', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(formData)
            });
            if (response.ok) {
                // Handle successful response
                console.log('Calculation successful');
                const data = await response.json();
                results.set(data);
                console.log($results);
                formSubmitted = true;
            } else {
                // Handle error response
                console.error('Calculation failed:', response.statusText);
            }
        } catch (error) {
            // Handle network error
            console.error('Network error:', error);
        }
    }

</script>

<ul>
    <li>
        Kalorit <input type="checkbox" name="calories" on:change={handleCheckboxChange}> <input type="number" name="calories" min="0" max="9999" on:input={handleInputChange}>
    </li>
    <li>
        Proteiini <input type="checkbox" name="protein" on:change={handleCheckboxChange}> <input type="number" name="protein" min="0" max="9999" on:input={handleInputChange}>
    </li>
    <li>
        Hiilihydraatit <input type="checkbox" name="carbohydrates" on:change={handleCheckboxChange}> <input type="number" name="carbohydrates" min="0" max="9999" on:input={handleInputChange}>
    </li>
    <li>
        Rasvat <input type="checkbox" name="fat" on:change={handleCheckboxChange}> <input type="number" name="fat" min="0" max="9999" on:input={handleInputChange}>
    </li>
    <li>
        Kuitu <input type="checkbox" name="fiber" on:change={handleCheckboxChange}> <input type="number" name="fiber" min="0" max="9999" on:input={handleInputChange}>
    </li>
</ul>

{#if formSubmitted}
    <h1 class="result-header">Tulos:</h1>
    <h2 style="font-weight: bold; text-decoration: underline;">Vuoden ruoka-aineet & niiden hinta</h2>
    <div class="results">
        <div class = left-col>
            {#each $results.foods as food}
            <p>{food.name}</p> <!-- Adjust this to display the specific data you want -->
            {/each}
        </div>
        <div class = right-col>
            {#each $results.foods as food}
            <p>{food.price.toFixed(2)}€</p> <!-- Adjust this to display the specific data you want -->
             {/each}
        </div> 
    </div>
    <h2 style="font-weight: bold; ">Yhteensä: {$results.optimalAnnualPrice.toFixed(2)}€</h2>

    <h2 style="font-weight: bold; margin-top: 20px; text-decoration: underline;">Ravinteet per päivä</h2>
    <div class="results">
        <div class = left-col>
                <!-- If we wanted to actually get every key name from the JSON, however this prints the names as they are in the database, in English.
                {#each Object.keys($results.nutrients) as nutrient}
                <p>{nutrient}</p>
                {/each} -->
            <p>Kalorit</p>    
            <p>Proteiini</p>    
            <p>Hiilihydraatit</p>    
            <p>Rasvat</p>    
            <p>Kuitu</p>    
        </div>
        <div class = right-col>
            {#each Object.keys($results.nutrients) as nutrient}
            <p>{$results.nutrients[nutrient].value.toFixed(2)} (g)</p> <!-- Adjust this to display the specific data you want -->
            {/each}
        </div> 
    </div>
{/if}
    
{#if !formSubmitted}
<div class="calc-button">
    <button on:click={handleSubmit}>Laske</button>
</div>
{:else}
<div class="calc-button">
    <button on:click={handleSubmit}>Laske uudelleen</button>
</div>
{/if}

<style>
    ul {
		padding: 0;
		margin: 0;
		list-style: none;
	}
    li {
		position: relative;
		height: 100%;
        font-size: 24px;
        padding-right: 300px;
	}
    input[type="checkbox"] {
        position:absolute;
        top: 20%;
        right: 150px; /* Checkbox distance from the right side of <li> */
        transform: translateY(-20%);
        width: 24px;
        height: 24px;
    }
    input[type="number"] {
      position: absolute;
      top: 20%;
      right: 0px;
      transform: translateY(-20%);
      width: 64px;
      height: 24px;  
    }
    input[type=number]::-webkit-inner-spin-button {
        -webkit-appearance: none; /* To Hide the up/arrow buttons from the number input box */
    }
    .calc-button {
        margin-top: 40px;
        text-align: center;
        cursor: pointer;
         
    }
    
    button {
        width: 180px; 
        height: 40px; 
        font-size: 18px;
        cursor: pointer;
         
    }
    .results {
    display: grid;
    grid-template-columns: repeat(2, auto); /* Adjust the number of columns as needed */
    gap: 10px; /* Gap between columns */
    }

    .left-col  {
        text-align: left;
    }
    .right-col {
        text-align: right;
    }
    .left-col p,
    .right-col p {
        margin: 5px 0; /* Adjust the margin to close the vertical gap */
    }
</style>