<template>
    <BaseLayout>
      <div v-if="!loading">
        <div class="chickens">
          <div class="chickens-item" v-for="chicken in chickens" :key="chicken.id">
            <div class="chickens-item__character">
              <span class="chickens-item__character-label"> Номер курицы </span> {{ chicken.id }}
            </div>
            <div class="chickens-item__character">
              <span class="chickens-item__character-label"> Вес </span> {{ chicken.weight }}
            </div>
            <div class="chickens-item__character">
              <span class="chickens-item__character-label"> Возраст </span> {{ chicken.age }}
            </div>
            <div class="chickens-item__character">
              <span class="chickens-item__character-label"> Яиц за месяц </span> {{ chicken.eggsPerMonth }}
            </div>
            <div class="chickens-item__character">
              <span class="chickens-item__character-label"> Порода </span> {{ chicken.breed.name }}
            </div>
            <div class="chickens-item__character">
              <span class="chickens-item__character-label"> Клетка </span> {{ chicken.cage.id }}
            </div>
          </div>
        </div>
      </div>
      <div v-else>
        Loading...
      </div>
    </BaseLayout>
  </template>
  
  <script>
  import BaseLayout from '@/layouts/BaseLayout.vue';
  import axios from 'axios';
  
  export default {
    name: 'MainView',
    components: {
      BaseLayout
    },
    data() {
      return {
        loading: true,
        chickens: []
      };
    },
    methods: {
      getchickenss() {
        axios.get('http://localhost:8080/api/chicken')
            .then(res => {
              this.chickens = res.data;
            })
            .catch(err => {
              console.log(err);
            })
            .finally(() => {
              this.loading = false;
            })
      }
    },
    mounted() {
      this.getchickenss();
    }
  };
  </script>
  
<style lang="scss" scoped>
.chickens {
  display: flex;
  flex-direction: column;
  gap: 15px;

  &-item{
    width: 100%;

    padding: 10px;
    border-radius: 10px;
    border: 1px solid #000;

    display: flex;
    flex-direction: row;
    gap: 5px;

    &__character{
      display: flex;
      flex-direction: column;
      gap: 5px;

      width: 15%;

      font-size: 14px;

      &-label{
        font-size: 10px !important;
        color: gray;
      }
    }
  }
}
</style>
  