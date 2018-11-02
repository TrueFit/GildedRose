const api = 'http://localhost:3000';

export async function find(filter) {
  const query = Object.keys(filter)
    .map(k => `${k}=${filter[k]}`)
    .join('&');

  const res = await fetch(`${api}/items?${query}`);
  const json = await res.json();
  if (res.ok) {
    return json;
  } else {
    throw new Error(json);
  }
}

export async function get(name) {
  const res = await fetch(`${api}/items/${name}`);
  if (res.ok) {
    return await res.json();
  } else if (res.status === 404) {
    throw new Error('Item not found.');
  } else {
    throw new Error(
      'There was a problem fetching this item. Please refresh to try again',
    );
  }
}

export async function advance() {
  const res = await fetch('${api}/items', {method: 'PATCH'});
  const json = await res.json();
  if (res.ok) {
    return json;
  } else {
    throw new Error(json);
  }
}
