<script>
	import Nutrients from './Nutrients.svelte';
</script>

<svelte:head>
	<title>Food optimizer</title>
	<meta name="description" content="Food optimizer" />
</svelte:head>

<section>
	<h1>
		Ravinteet
	</h1>

	<Nutrients />
	
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
</style>
