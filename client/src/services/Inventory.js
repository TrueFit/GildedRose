export async function find(filter) {
  const query = Object.keys(filter)
    .map(k => `${k}=${filter[k]}`)
    .join('&');

  const res = await fetch(`http://localhost:3000/items?${query}`);
  const json = await res.json();
  return json;
}
