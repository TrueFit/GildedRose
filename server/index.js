const app = require('./src/app');
const inventory = require('./src/inventory');
const port = 3000;

inventory.init().then(() => {
  app.listen(port, () =>
    console.log(`Gilded Rose server running on port ${port}`),
  );
});
