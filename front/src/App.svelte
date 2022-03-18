<script>
	var a = new URLSearchParams(window.location.search)
	let query = a.get('query')

	if(query) {
		fetch('http://localhost:9000/Pokemon/Search?name=pokemon A')
		.then(response => {
			if(response.ok) return response.json()

			throw new Error
		})
		.then(data => {
			
		})
		.catch(err => {
			console.error('ECAA')
			alert('API Down');
			document.location = '/'
		})
		console.log('search')
	}
</script>

<main>
	<h1>Dev Space</h1>
	{#if query}
	<h3>{query}</h3>
	{:else}
	<form>
		<input type="text" name="query" placeholder="#pokemon name" autocomplete="off" required>
		<button type="submit">Buscar</button>
	</form>
	{/if}
</main>

<style>
	main {
		text-align: center;
		padding: 1em;
		max-width: 240px;
		margin: 0 auto;
	}

	h1 {
		color: #ff3e00;
		text-transform: uppercase;
		font-size: 4em;
		font-weight: 100;
	}

	@media (min-width: 640px) {
		main {
			max-width: none;
		}
	}
</style>