<template>
    <div class="header footer">
        <div class="copyright">
            <p><b>FreeStore ©</b></p>
            <p>Darbą atliko <b>Ernestas Untulis IFF-9/7</b></p>
        </div>
        <div class="mainTitle">
            <h1>FreeStore</h1>
        </div>
    </div>
</template>

<script>
export default {

}
</script>

<style>
.header.footer {
    background-color: #627575;
    margin: auto;
    margin-top: 150px;
    margin-bottom: 30px;
    min-height: 100px;
    max-width: 1200px;
}

.copyright {
    margin: 20px;
    float: right;
    text-align: right;
}

.header.footer p{
    color: rgb(200, 255, 255);
}

@media (max-width: 768px) {
    div .copyright {
        padding-top: 40px;
        float: none;
        text-align: center;
    }
}

</style>