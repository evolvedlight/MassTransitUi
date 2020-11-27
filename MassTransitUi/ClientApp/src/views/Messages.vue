<template>
  <div>
    <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
      <h1 class="h2">Failed Messages</h1>
      <div class="btn-toolbar mb-2 mb-md-0">
        <div class="btn-group mr-2">
          <button type="button" @click="retryAll" class="btn btn-sm btn-outline-secondary">Retry All</button>
          <button type="button" @click="deleteAll" class="btn btn-sm btn-outline-secondary">Delete All</button>
        </div>
        <!-- <div class="btn-group mr-2">
          <button type="button" class="btn btn-sm btn-outline-secondary">Share</button>
          <button type="button" class="btn btn-sm btn-outline-secondary">Export</button>
        </div>
        <button type="button" class="btn btn-sm btn-outline-secondary dropdown-toggle">
          <span data-feather="calendar"></span>
          This week
        </button> -->
      </div>
    </div>
    <MessageList :filteredMessages="filteredMessages"/>
  </div>
</template>

<script>
import axios from 'axios';
import { HubConnectionBuilder, LogLevel } from '@aspnet/signalr';
import MessageList from '../components/MessageList.vue';

export default {
  name: 'Messages',
  props: {
    messages: Array,
    quickFilter: String,
  },
  data () {
    return {
      messages: []
    }
  },
  created () {
    const connection = new HubConnectionBuilder()
      .withUrl('/errorQueueHub')
      .configureLogging(LogLevel.Information)
      .build()

    connection.on('NewErrorInQueue', (messageStr) => {
      var messageObj = JSON.parse(messageStr);
      messageObj.Status = "new";
      this.messages.push(messageObj);
    })
 
    connection.start()
  },
  methods: {
    deleteAll() {
      this.messages.forEach(message => {
        if (message.Status != "retried" && message.Status !== 'deleted') {
            axios.post('/api/message/' + message.Id + "/delete", { })
          .then(function (response) {
            message.Status = "deleted"
            console.log(response);
          })
          .catch(function (error) {
            message.Status = "error on delete"
            console.log(error);
          });
        }
      });
    },
    retryAll() {
      this.messages.forEach(message => {
        if (message.Status != "retried" && message.Status !== 'deleted') {
            axios.post('/api/message/' + message.Id + "/retry", { })
          .then(function (response) {
            message.Status = "retried"
            console.log(response);
          })
          .catch(function (error) {
            message.Status = "error on retry"
            console.log(error);
          });
        }
      });
    }
  },
  computed: {
    filteredMessages() {
      return this.messages.filter(
        m => m.Queue.toLowerCase().includes(this.quickFilter.toLowerCase())
        || m.MessageId.toLowerCase().includes(this.quickFilter.toLowerCase())
        || m.ErrorMessage?.toLowerCase()?.includes(this.quickFilter.toLowerCase())
        || m.Content?.toLowerCase()?.includes(this.quickFilter.toLowerCase())

      )
    }
  },
  components: {
    MessageList
  }
}
</script>
