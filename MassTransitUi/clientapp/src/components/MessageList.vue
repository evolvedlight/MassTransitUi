<template>
  <div>
    <p>You have {{ messages.length }} messages</p>
    <table>
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
          <td><a @click="retryMessage(message)">Retry</a> {{ message.Status }} </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  name: 'MessageList',
  props: {
    messages: Array,
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
