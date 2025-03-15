<template>
  <BaseLayout>
    <div class="statistics">
      <div class="statistics-title">
        Статистика
      </div>
      <div class="statistics-items">
        <div class="statistics-items__filter">
          <div class="statistics-items__filter-title">
            Cредние данные по курицам
          </div>
          <form class="statistics-items__filter-form" @submit.prevent="getFilterData()">
            <input v-model="formFilter.minWeight" type="number" step="0.1" placeholder="Минимальный вес" required>
            <input v-model="formFilter.maxWeight" type="number" step="0.1" placeholder="Максимальный вес" required>
            <input v-model="formFilter.minAge" type="number" step="0.1" placeholder="Минимальный возраст" required>
            <input v-model="formFilter.maxAge" type="number" step="0.1" placeholder="Максимальный возраст" required>
            <div>
              <button class="btn-primary" type="submit"> Запросить </button>
            </div>
          </form>
          <div class="errors" v-if="averageEggsErrors">
            {{averageEggsErrors}}
          </div>
          <div v-if="averageEggs" class="statistics-items__filter-answer">
            Среднее количество яиц снесенных курацами по заданным параметрам <b>{{averageEggs}}</b>
          </div>
        </div>

        <div class="statistics-items__low">
          <div class="statistics-items__low-title">
            Курицы снесшие меньше яиц чем, средняя яйценосность по фабрике
          </div>
          <div v-if="lowChickenLoading">Loading...</div>
          <div v-else class="chickens">
            <div class="chickens-item" v-for="chicken in lowChickens" :key="chicken.id">
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

        <div class="statistics-items__highest">
          <div class="statistics-items__highest-title">
            Клетка с наилучшим показателем
          </div>
          <div v-if="highestCageLoading">
            Loading...
          </div>
          <div v-else class="statistics-items__highest-answer">
            Клетка с наилучшим показателем <b>{{highestCage.id}}</b>
            <br>
            Сотрудник <b>{{highestCage.employee.fullName}}</b>
          </div>
        </div>

        <div class="statistics-items__employee-eggs">
          <div class="statistics-items__employee-eggs-title">
            Количество яиц, собранных каждым работником
          </div>
          <div v-if="eggsPerEmployeesLoading">Loading...</div>
          <div v-else class="chickens">
            <div class="chickens-item" v-for="eggsPerEmployee in eggsPerEmployees" :key="eggsPerEmployee.id">
              <div class="chickens-item__character">
                <span class="chickens-item__character-label"> Номер сотрудника </span> {{ eggsPerEmployee.id }}
              </div>
              <div class="chickens-item__character">
                <span class="chickens-item__character-label"> Имя </span> {{ eggsPerEmployee.fullName }}
              </div>
              <div class="chickens-item__character">
                <span class="chickens-item__character-label"> Яица </span> {{ eggsPerEmployee.eggs }}
              </div>
            </div>
          </div>
        </div>

        <div class="statistics-items__employee-chickens">
          <div class="statistics-items__employee-eggs-title">
            Количество куриц, обслуживаемыех каждым работником
          </div>
          <div v-if="chickensPerEmployeesLoading">Loading...</div>
          <div v-else class="chickens">
            <div class="chickens-item" v-for="chickensPerEmployee in chickensPerEmployees" :key="chickensPerEmployee.id">
              <div class="chickens-item__character">
                <span class="chickens-item__character-label"> Номер сотрудника </span> {{ chickensPerEmployee.id }}
              </div>
              <div class="chickens-item__character">
                <span class="chickens-item__character-label"> Имя </span> {{ chickensPerEmployee.fullName }}
              </div>
              <div class="chickens-item__character">
                <span class="chickens-item__character-label"> Курицы </span> {{ chickensPerEmployee.chickens }}
              </div>
            </div>
          </div>
        </div>

        <div class="statistics-items__price">
          <div class="statistics-items__price-title">
            Cредние данные по курицам
          </div>
          <form class="statistics-items__price-form" @submit.prevent="getPriceDataData()">
            <input v-model="formPrice.pricePerEgg" type="number" step="0.1" placeholder="Цена за яйцо" required>
            <div>
              <button class="btn-primary" type="submit"> Запросить </button>
            </div>
          </form>
          <div class="errors" v-if="allPriceErrors">
            {{allPriceErrors}}
          </div>
          <div v-if="allPrice" class="statistics-items__price-answer">
           Всего яиц: <b>{{allPrice.total_eggs}}</b>
           Цена за яицо: <b>{{allPrice.price_per_egg}}</b>
           Общая цена: <b>{{allPrice.total_price}}</b>
          </div>
        </div>
      </div>
    </div>

  </BaseLayout>
</template>

<script>
import BaseLayout from '@/layouts/BaseLayout.vue';
import axios from 'axios';
import Swal from "sweetalert2";

export default {
  name: 'MainView',
  components: {
    BaseLayout
  },
  data() {
    return {
      formFilter:{
        minAge: null,
        maxAge: null,
        minWeight: null,
        maxWeight: null,
      },
      averageEggs: null,
      averageEggsErrors: null,

      lowChickenLoading: true,
      lowChickens: [],

      highestCageLoading: true,
      highestCage: null,

      eggsPerEmployeesLoading: true,
      eggsPerEmployees: null,

      chickensPerEmployeesLoading: true,
      chickensPerEmployees: null,

      formPrice:{
        pricePerEgg: null,
      },
      allPrice: null,
      allPriceErrors: null,
    };
  },
  methods: {
    getFilterData(){
      axios.get('http://localhost:8080/api/chicken/average-eggs', {params: this.formFilter})
          .then(res=>{
            this.averageEggs = res.data.averageEggs;
            this.averageEggsErrors = null;
          })
          .catch(err=>{
            this.averageEggsErrors = err.response.data.message;
            this.averageEggs = null;
            console.log(err)
          })
    },
    getLowChickens(){
      axios.get('http://localhost:8080/api/chicken/low-average')
          .then(res=>{
            this.lowChickens = res.data;
            this.lowChickenLoading = false;
          })
          .catch(err=>{
            console.log(err)
            this.lowChickenLoading = false;
          })
    },
    getHighestCage(){
      axios.get('http://localhost:8080/api/chicken/highest')
          .then(res=>{
            console.log(res)
            this.highestCage = res.data;
            this.highestCageLoading = false;
          })
          .catch(err=>{
            console.log(err)
            this.highestCageLoading = false;
          })
    },
    getEggsByEmployees(){
      axios.get('http://localhost:8080/api/employee/GetEggsPerEmployee')
          .then(res=>{
            this.eggsPerEmployees = res.data;
            this.eggsPerEmployeesLoading = false;
          })
          .catch(err=>{
            console.log(err)
            this.eggsPerEmployeesLoading = false;
          })
    },
    getChickensByEmployees(){
      axios.get('http://localhost:8080/api/employee/GetChickensPerEmployee')
          .then(res=>{
            this.chickensPerEmployees = res.data;
            this.chickensPerEmployeesLoading = false;
          })
          .catch(err=>{
            console.log(err)
            this.chickensPerEmployeesLoading = false;
          })
    },
    getPriceDataData(){
      axios.get('http://localhost:8080/api/employee/GetAllPriceEggs', {params: this.formPrice})
          .then(res=>{
            this.allPrice = res.data.data;
            this.allPriceErrors = null;
          })
          .catch(err=>{
            this.allPriceErrors = err.response.data.message;
            this.allPrice = null;
            console.log(err.response.data)
          })
    }
  },
  mounted() {
    this.getLowChickens();
    this.getHighestCage();
    this.getEggsByEmployees();
    this.getChickensByEmployees()
  }
};
</script>


<style lang="scss" scoped>
  .statistics {
    display: flex;
    flex-direction: column;
    gap: 15px;

    &-title{
      font-size: 24px;
      font-weight: 600;
    }

    &-items{
      display: flex;
      flex-direction: column;
      gap: 35px;

      &__filter{
        display: flex;
        flex-direction: column;
        gap: 15px;

        &-title{
          font-size: 18px;
          font-weight: 600;
        }

        &-form{
          display: flex;
          flex-direction: row;
          gap: 15px;
          flex-wrap: wrap;
        }
      }

      &__low{
        display: flex;
        flex-direction: column;
        gap: 15px;

        &-title{
          font-size: 18px;
          font-weight: 600;
        }

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
      }

      &__highest{
        display: flex;
        flex-direction: column;
        gap: 15px;

        &-title{
          font-size: 18px;
          font-weight: 600;
        }
      }

      &__employee-eggs{
        display: flex;
        flex-direction: column;
        gap: 15px;

        &-title{
          font-size: 18px;
          font-weight: 600;
        }

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
      }

      &__employee-chickens{
        display: flex;
        flex-direction: column;
        gap: 15px;

        &-title{
          font-size: 18px;
          font-weight: 600;
        }

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
      }

      &__price{
        display: flex;
        flex-direction: column;
        gap: 15px;

        &-title{
          font-size: 18px;
          font-weight: 600;
        }

        &-form{
          display: flex;
          flex-direction: row;
          gap: 15px;
          flex-wrap: wrap;
        }
      }
    }
  }
  .errors{
    font-weight: 600;
    color: #910303;
  }
</style>
  