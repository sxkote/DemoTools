<template>
    <div v-if="isShown">
        <transition name="modal">
            <div class="modal-mask">
                <div class="modal-wrapper">
                    <div class="modal-dialog" role="document">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title">{{model.caption}}</h5>
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true" @click="$emit('close', null);">×</span>
                                </button>
                            </div>
                            <div class="modal-body">
                                <div class="form-group">
                                    <input type="text" :placeholder="model.placeholder" class="form-control" v-model="model.title" required />
                                </div>
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-secondary" @click="$emit('close', null);">Close</button>
                                <button type="button" class="btn btn-primary" @click="$emit('close', model);">Apply</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </transition>
    </div>
</template>


<script lang="ts">
    import { Vue, Options } from 'vue-class-component';
    import { Prop } from 'vue-property-decorator';

    @Options({
        props: {
            caption: String,
            isShown: Boolean,
        },
        emits: {
            'close': null
        }
    })
    export default class TitleModal extends Vue {
        @Prop()
        model: any = { title: "" };
    }
</script>

