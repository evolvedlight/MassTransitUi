<template>
  <div>
    <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
      <h1 class="h2">Failed Messages</h1>
      <div class="btn-toolbar mb-2 mb-md-0">
        <div class="btn-group mr-2">
          <button type="button" class="btn btn-sm btn-outline-secondary">Share</button>
          <button type="button" class="btn btn-sm btn-outline-secondary">Export</button>
        </div>
        <button type="button" class="btn btn-sm btn-outline-secondary dropdown-toggle">
          <span data-feather="calendar"></span>
          This week
        </button>
      </div>
    </div>
    <div class="table-responsive">
      <table class="table table-striped table-hover">
        <thead>
          <th>Id</th>
          <th>Queue</th>
          <th>MessageId</th>
          <th>Fault</th>
          <th>ErrorMessage</th>
          <th>Actions</th>
        </thead>
        <tbody>
          <tr v-for="message in messages" :key="message.Id">
            <td>{{ message.Id }}</td>
            <td>{{ message.Queue }}</td>
            <td>{{ message.MessageId }}</td>
            <td>{{ message.Fault }}</td>
            <td>{{ message.ErrorMessage }}</td>
            <td>
              <div v-if="message.Status != 'retried'">
                <a class="btn btn-primary" @click="retryMessage(message)">Retry</a>
              </div>
              <div v-else class="alert alert-success" role="alert">
                Successfully retried
              </div>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script>
import axios from 'axios';
import { HubConnectionBuilder, LogLevel } from '@aspnet/signalr'

export default {
  name: 'Messages',
  props: {
    messages: Array,
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

    connection.on('NewErrorInQueue', (messageObj) => {
        this.messages.push(JSON.parse(messageObj))
    })
 
    connection.start()
  },
  methods: {
    retryMessage(message) {
      message.Status = "retrying";
      console.log("retrying" + message.MessageId);

      axios.post('/api/message/' + message.Id + "/retry", {
      })
      .then(function (response) {
        message.Status = "retried"
        console.log(response);
      })
      .catch(function (error) {
        message.Status = "error on retry"
        console.log(error);
      });
    }
  }
}
</script>
