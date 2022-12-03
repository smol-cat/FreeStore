<template>
    <div class="itemDetailsScreen">
        <div v-if="item" class="details">
            <p>Prekės informacija</p>
            <h1>{{ item.title }}</h1>
            <div class="item">
                <div class="price">
                    <p>{{ item.price.toFixed(2) }} €</p>
                </div>
                <div class="description">
                    <p>{{ item.description }}</p>
                </div>
                <div class="buttonRow">
                    <div v-if="this.ableToEdit()" style="margin-right: 10px">
                        <block-button :text="'redaguoti'" :onClick="() => openEdit()" />
                    </div>
                    <div v-if="this.ableToDelete()">
                        <block-button :text="'šalinti'" :onClick="() => this.$emit('triggerModal', 'Tikrai norite pašalinti šį skelbimą?', deleteItem)" />
                    </div>
                </div>
                <div class="itemBottom">
                    <a>Paskelbė: {{ item.account.name + " " + item.account.last_name }}</a>
                    <p>{{ item.state.name }}</p>
                </div>
            </div>

        </div>
        <div v-if="comments" class="commentSection">
            <p>Komentarai</p>
            <newComment />
            <commentItem v-for="comment in comments" :key="comment.id" :comment="comment" @triggerModal="(text, onConfirm) => this.$emit('triggerModal', text, onConfirm)" />
        </div>
    </div>
</template>

<script>
import commentItem from '@/components/informational/commentItem.vue'
import newComment from '@/components/input/newComment.vue';
import blockButton from '@/components/input/blockButton.vue';

export default {
    data() {
        return {
            item: null,
            comments: null
        }
    },
    components: {
        commentItem,
        newComment,
        blockButton
    },
    async beforeMount() {
        this.loadItem();
        this.loadComments();
    },
    emits: [ 'triggerModal' ],
    methods: {
        async loadItem() {
            var response = await this.performRequest(location.pathname, "GET")
            if (response.success) {
                this.item = response.body
            }
        },
        async loadComments() {
            var endpoints = location.pathname.split('/')
            var response = await this.performRequest(`/categories/${endpoints[2]}/items/${endpoints[4]}/comments`, "GET")
            if (response.success) {
                this.comments = response.body
            }
        },
        ableToDelete() {
            return this.item.account.id.toString() === sessionStorage["userId"] || sessionStorage["userLevel"] === '1'
        },
        ableToEdit() {
            return this.item.account.id.toString() === sessionStorage["userId"]
        },
        openEdit(){
            var endpoints = location.pathname.split('/')
            location.pathname = `/categories/${endpoints[2]}/items/${endpoints[4]}/edit`
        },
        async deleteItem(){
            var endpoints = location.pathname.split('/')
            var response = await this.performRequest(`/categories/${endpoints[2]}/items/${endpoints[4]}`, "DELETE")
            if(response.success){
                location.pathname = `/categories/${endpoints[2]}/items`
            }
        }
    }
}
</script>

<style>
.itemDetailsScreen {
    display: block;
    max-width: 500px;
    margin: auto;
    text-align: left;
    margin-top: 20px;
}

.details>p {
    color: #a1b6ca;
    margin-left: 10px
}

.details h1 {
    font-size: 1.5em;
    margin-left: 10px
}

.description p {
    color: rgb(44, 42, 70);
    margin-bottom: 30px;
}

.commentSection {
    margin-top: 20px;
    text-align: left;
}

.commentSection>p {
    margin-left: 10px;
    margin-bottom: 20px;
    color: #a1b6ca;
}

.buttonRow {
    display: inline-flex;
    float: right;
}
</style>