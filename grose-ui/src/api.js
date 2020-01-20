// TODO replace this mock data with fetch hookups

export const getAllItems = () => {
    return {'inventory': [
        {
          'name': 'hello',
          'category': 'howareyou',
          'sellIn': 0,
          'quality': 0,
        },
    ]};
}
export const getTrash = () => {
    return {'inventory': [
        {
          'name': 'trash',
          'category': 'trash',
          'sellIn': 0,
          'quality': 0,
        },
    ]};
}

export const postAdvanceDay = () => {
    return {'results': [
        {
          'name': 'hello',
          'category': 'howareyou',
          'sellIn': 0,
          'quality': 0,
        },
    ]};
}