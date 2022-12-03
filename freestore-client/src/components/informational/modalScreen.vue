<template>
    <div class="modal">
        <div class="item modalContent">
            <p>{{ this.message }}</p>
            <table class="modalButtonRow">
                <tr>
                    <td class="modalButton">
                        <block-button text="Atšaukti" :onClick="this.close" />
                    </td>
                    <td class="modalButton">
                        <block-button text="Patvirtinti" :onClick="this.confirmAndClose" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</template>

<script>
import blockButton from '../input/blockButton.vue'
export default {
    components: { blockButton },
    props: {
        message: String,
        onConfirm: Function
    },
    methods: {
        close() {
            this.$emit("closeModal")
        },
        confirmAndClose() {
            this.onConfirm()
            this.close()
        }
    }
}
</script>

<style>
.modal {
    position: fixed;
    z-index: 1001;
    left: 0;
    top: 0;
    width: 100%;
    height: 100%;
    overflow: auto;
    background-color: rgb(0, 0, 0);
    background-color: rgba(0, 0, 0, 0.4);
    box-shadow: 0px 5px 20px 0px #000000;
}

.item.modalContent {
    max-width: fit-content;
    margin: 15% auto;
    padding: 20px;
}

.modalButtonRow {
    margin-top: 30px;
    width: 100%;
}

.modalButtonRow td {
    display: inline-flex;
    width: 49%;
    align-items: center;
    align-content: center;
}
</style>