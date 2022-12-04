<template>
    <h1>Namai</h1>
    <div class="item home">
        <h3>Sveiki atvykę į FreeStore tinklapį!</h3>
        <p>Pirkite, skelbkite, parduokite įvairius produktus ir paslaugas.</p>
        <p>Norint stebėti ir kurti skelbimus, prisijunkite. Jei neturite paskyros, prisiregistruokite.</p>
        <img src="people_trading.webp" alt="people trading" />
    </div>
</template>

<script>
export default {
    name: 'homeScreen',
    emits: ['triggerModal'],
    screen: true
}
</script>

<style>
fieldset {
    border: 0;
}

.home {
    box-sizing: content-box;
    max-width: 500px;
}

.home h3 {
    color: rgb(44, 42, 70);
}

.home p {
    margin-bottom: 5px;
}

.home img {
    width: 100%;
    height: auto;
    vertical-align: middle;
    border-radius: 10px;
    margin-top: 20px;
}
</style>
