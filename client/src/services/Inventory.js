export async function find(filter) {
  const query = Object.keys(filter)
    .map(k => `${k}=${filter[k]}`)
    .join('&');

  const res = await fetch(`http://localhost:3000/items?${query}`);
  const json = await res.json();
  if (res.ok) {
    return json;
  }
  else {
    throw Error(json);
  }
}

export async function advance() {
  const res = await fetch('http://localhost:3000/items', { method: 'PATCH' });
  const json = await res.json();
  if (res.ok) {
    return json;
  }
  else {
    console.log('dddd');
    throw Error(json);
  }
}
