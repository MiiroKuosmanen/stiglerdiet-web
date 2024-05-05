<script>
    import { onMount } from 'svelte';

    let current = '/';
    const initializeCurrent = () => {
        const currentPath = window.location.pathname;
        current = currentPath === '/' ? '/' : '/ruuat';
    };

    onMount(initializeCurrent);

</script>

<header>
    <div class="sidenav">
        <a href="/" class:selected={current === '/'} on:click={() => (current = '/')}>Laske</a>
        <a href="/ruuat" class:selected={current === '/ruuat'} on:click={() => (current = '/ruuat')}>Ruuat</a>
    </div>
</header>

<style scoped>
    .sidenav {
        height: 100%;
        width: 160px;
        position: fixed;
        z-index: 1;
        top: 0;
        left: 0;
        background-color: #111;
        overflow-x: hidden; /* Disable horizontal scroll */
        padding-top: 20px;
    }

    .sidenav a {
        padding: 6px 8px 6px 16px;
        text-decoration: none;
        font-size: 40px;
        color: #818181;
        display: block;
        text-align: center;
        background-color: transparent;
        
    }

    .sidenav a:hover {
        color: #f1f1f1;
    }

    /* Custom class for selected/active page */
    .sidenav a.selected {
        background-color: green; /* Change to desired active background color */
        color: black;
    }
</style>
